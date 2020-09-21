using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Extensions;
using ApplicationCore.Models;
using ApplicationCore.Repositories;

namespace Web.Services.Interfaces
{
    public class OfferPromotionService : IOfferPromotionService
    {
        private readonly IBaseRepository<OfferPromotion> offerPromotionRepository;
        private readonly IBaseRepository<Offer> offerRepository;

        public OfferPromotionService(IBaseRepository<OfferPromotion> offerPromotionRepository,
            IBaseRepository<Offer> offerRepository)
        {
            this.offerPromotionRepository = offerPromotionRepository;
            this.offerRepository = offerRepository;
        }

        public async Task<bool> PromoteOffer(Guid id, string tag)
        {
            var tagsAvailable = GetTagsAvailableForPromotion(new List<string> { tag });
            var offer = offerRepository.GetById(id);
            if (offer == null || !tagsAvailable.Any() || !tagsAvailable.ContainsKey(tag))
            {
                return false;
            }

            await offerPromotionRepository.AddAsync(new OfferPromotion
            {
                EndDate = DateTime.Now.AddDays(1),
                Offer = offer,
                PromotedTag = tag
            });
            return true;
        }

        public Dictionary<string, decimal> GetTagsAvailableForPromotion(IEnumerable<string> tags)
        {
            var result = new Dictionary<string, decimal>();
            var availableTags = tags.Except(offerPromotionRepository.GetAll().ToList().Where(o => tags.Contains(o.PromotedTag) && o.EndDate.IsLater(DateTime.Now)).Select(o => o.PromotedTag).ToList()).Distinct();
            foreach (var tag in availableTags)
            {
                result.Add(tag, GetPromotionPrice(tag));
            }

            return result;
        }

        public Offer GetPromotedOffer(IEnumerable<string> tags)
        {
            var offers = offerPromotionRepository.GetAll().ToList()
                .Where(o => tags.Contains(o.PromotedTag) && o.EndDate.IsLater(DateTime.Now))
                .Select(o => o.Offer);
            return offers.Any() ? offers.GetRandom() : null;
        }

        private decimal GetPromotionPrice(string tag)
        {
            return 30 + (decimal)Math.Sqrt(offerRepository.GetAll().ToList().Count(o => o.Tags.Contains(tag) && !o.IsHidden && !o.IsBlocked));
        }
    }
}
