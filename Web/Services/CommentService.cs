using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using ApplicationCore.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class CommentService : ExtendedBaseService<Comment>, ICommentService
    {
        protected override int ResultsPerPage => 20;

        public CommentService(IBaseRepository<Comment> repository, IMapper mapper) : base(repository, mapper)
        {

        }

        public override IQueryable<Comment> GetAll()
        {
            return base.GetAll().Include(comment => comment.CreatedBy);
        }

        public Comment GetByIdAsNoTracking(Guid id)
        {
            return repository.GetAll().Where(offer => offer.Id == id).Include(offer => offer.CreatedBy).AsNoTracking()
                .FirstOrDefault();
        }

        public PagedResult<CommentVm> GetPagedAndFiltered(int page, string? profileUserName, Guid? offerId)
        {
            var query = repository.GetAll().OrderByDescending(comment => comment.CreatedDate).Include(comment => comment.CreatedBy).AsQueryable();

            if (profileUserName != null)
            {
                query = query.Where(comment => comment.Offer.CreatedBy.UserName == profileUserName);
            }
            if (offerId != null)
            {
                query = query.Where(comment => comment.Offer.Id == offerId);
            }
            return new PagedResult<CommentVm>
            {
                Items = mapper.Map<List<CommentVm>>(query.Skip((page - 1) * ResultsPerPage).Take(ResultsPerPage).ToList()),
                CurrentPage = page,
                ResultsPerPage = ResultsPerPage,
                PagesCount = query.Any() ?
                    (query.Count() - 1) / ResultsPerPage + 1 :
                    0
            };
        }

        public async Task SetToPositive(Guid id)
        {
            var comment = GetById(id);
            if (comment == null)
            {
                throw new ElementNotFoundException("Comment not found");
            }

            comment.IsPositive = true;
            await repository.UpdateAsync(id, comment);
        }
    }
}
