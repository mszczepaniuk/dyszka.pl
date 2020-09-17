using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.ViewModels;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Web.Services.Interfaces
{
    public interface ICommentService : IExtendedBaseService<Comment>
    {
        public Comment GetByIdAsNoTracking(Guid id);
        public PagedResult<CommentVm> GetPagedAndFiltered(int page, string? profileUserName, Guid? offerId);
        public Task SetToPositive(Guid id);
    }
}
