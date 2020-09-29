using System;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.ViewModels;

namespace ApplicationCore.Services
{
    public interface ICommentService : IExtendedBaseService<Comment>
    {
        public Comment GetByIdAsNoTracking(Guid id);
        public PagedResult<CommentVm> GetPagedAndFiltered(int page, string? profileUserName, Guid? offerId);
        public Task SetToPositive(Guid id);
    }
}
