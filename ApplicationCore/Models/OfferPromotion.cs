using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Models.Interfaces;

namespace ApplicationCore.Models
{
    public class OfferPromotion : BaseEntity, ICreatedDate
    {
        public DateTime CreatedDate { get; set; }
        public Offer Offer { get; set; }
        public string PromotedTag { get; set; }
        public DateTime EndDate { get; set; }
    }
}
