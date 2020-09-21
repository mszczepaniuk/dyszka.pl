using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace Web.Services.Interfaces
{
    public interface IOfferPromotionService
    {
        public Task<bool> PromoteOffer(Guid id, string tag);
        public Dictionary<string, decimal> GetTagsAvailableForPromotion(IEnumerable<string> tags);
        public Offer GetPromotedOffer(IEnumerable<string> tags);
    }
}