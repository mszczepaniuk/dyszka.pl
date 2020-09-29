using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.ViewModels;

namespace ApplicationCore.Services
{
    public interface IOfferService : IExtendedBaseService<Offer>
    {
        public Offer GetByIdLazy(Guid id);
        public PagedResult<OfferVm> GetPagedAndFiltered(int page, IEnumerable<string> tags, string username);
        public Task HideOffer(Guid id);
        public Task ShowOffer(Guid id);
    }
}
