using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.ViewModels;

namespace Web.Services.Interfaces
{
    public interface IOfferService : IExtendedBaseService<Offer>
    {
        public PagedResult<OfferVm> GetPagedAndFiltered(int page, IEnumerable<string> tags, string username);
        public Task BlockUserOffers(string username);
        public Task UnBlockUserOffers(string username);
        public Task HideOffer(Guid id);
        public Task ShowOffer(Guid id);
    }
}
