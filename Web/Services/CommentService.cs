using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Exceptions;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using ApplicationCore.Services;
using ApplicationCore.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Web.Services
{
    public class CommentService : ExtendedBaseService<Comment>, ICommentService
    {
        private readonly IUserService userService;
        private readonly IAuditLogService auditLogService;
        protected override int ResultsPerPage => 20;

        public CommentService(IBaseRepository<Comment> repository,
            IMapper mapper,
            IUserService userService,
            IAuditLogService auditLogService) : base(repository, mapper)
        {
            this.userService = userService;
            this.auditLogService = auditLogService;
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
            if (offerId.HasValue)
            {
                query = query.Where(comment => comment.Offer.Id == offerId.Value);
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

        public override Task<bool> RemoveAsync(Guid id)
        {
            if (GetByIdAsNoTracking(id).CreatedBy.UserName != userService.CurrentUser.UserName)
            {
                auditLogService.AddAuditLogAsync("Usunięto komentarz", id);
            }
            return base.RemoveAsync(id);
        }
    }
}
