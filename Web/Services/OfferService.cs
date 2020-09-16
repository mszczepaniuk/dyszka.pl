using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using ApplicationCore.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class OfferService : ExtendedBaseService<Offer>, IOfferService
    {
        private readonly IUserService userService;

        public OfferService(IBaseRepository<Offer> repository,
            IMapper mapper,
            IUserService userService) : base(repository, mapper)
        {
            this.userService = userService;
        }

        public override Offer GetById(Guid id)
        {
            return repository.GetAll().Where(offer => offer.Id == id).Include(offer => offer.CreatedBy)
                .FirstOrDefault();
        }

        public PagedResult<OfferVm> GetPagedAndFiltered(int page, IEnumerable<string> tags, string username)
        {
            var query = repository.GetAll().OrderByDescending(offer => offer.CreatedDate).Include(offer => offer.CreatedBy).AsQueryable();
            
            if (username == null || userService.CurrentUser == null || username != userService.CurrentUser.UserName)
            {
                query = query.Where(offer => !offer.IsBlocked && !offer.IsHidden);
            }
            if (username != null)
            {
                query = query.Where(offer => offer.CreatedBy.UserName == username);
            }
            if (tags != null && tags.Any())
            {
                query = query.ToList().Where(offer => offer.Tags.Intersect(tags).Any()).AsQueryable();
            }
            return new PagedResult<OfferVm>
            {
                Items = mapper.Map<List<OfferVm>>(query.Skip((page - 1) * ResultsPerPage).Take(ResultsPerPage).ToList()),
                CurrentPage = page,
                ResultsPerPage = ResultsPerPage,
                PagesCount = query.Any() ?
                    (query.Count() - 1) / ResultsPerPage + 1 :
                    0
            };
        }

        public async Task HideOffer(Guid id)
        {
            var offer = repository.GetById(id);
            offer.IsHidden = true;
            await repository.UpdateAsync(offer.Id, offer);
        }

        public async Task ShowOffer(Guid id)
        {
            var offer = repository.GetById(id);
            offer.IsHidden = false;
            await repository.UpdateAsync(offer.Id, offer);
        }
    }
}
