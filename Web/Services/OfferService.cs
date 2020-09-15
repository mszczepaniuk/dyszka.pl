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
        public OfferService(IBaseRepository<Offer> repository, IMapper mapper) : base(repository, mapper)
        {
            
        }

        public PagedResult<OfferVm> GetPagedAndFiltered(int page, IEnumerable<string> tags, string username)
        {
            var query = repository.GetAll().Include(offer => offer.CreatedBy).AsQueryable();
            if (tags != null && tags.Any())
            {
                query = query.Where(offer => offer.Tags.Any(tag => tags.Contains(tag)));
            }
            if (username != null)
            {
                query = query.Where(offer => offer.CreatedBy.UserName == username);
            }
            return new PagedResult<OfferVm>
            {
                Items = mapper.Map<List<OfferVm>>(query.Skip((page - 1) * resultsPerPage).Take(resultsPerPage).ToList()),
                CurrentPage = page,
                ResultsPerPage = resultsPerPage,
                PagesCount = query.Any() ?
                    (query.Count() - 1) / resultsPerPage + 1 :
                    0
            };
        }

        public async Task BlockUserOffers(string username)
        {
            foreach (var offer in repository.GetAll().Where(offer => offer.CreatedBy.UserName == username))
            {
                offer.IsBlocked = true;
                await repository.UpdateAsync(offer.Id, offer);
            }
        }

        public async Task UnBlockUserOffers(string username)
        {
            foreach (var offer in repository.GetAll().Where(offer => offer.CreatedBy.UserName == username))
            {
                offer.IsBlocked = false;
                await repository.UpdateAsync(offer.Id, offer);
            }
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
