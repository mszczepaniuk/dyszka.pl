using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Models;

namespace ApplicationCore.Services
{
    public interface IOfferPromotionService
    {
        public Task<bool> PromoteOffer(Guid id, string tag);
        public Dictionary<string, decimal> GetTagsAvailableForPromotion(Guid id, IEnumerable<string> tags);
        public Offer GetPromotedOffer(IEnumerable<string> tags);
    }
}