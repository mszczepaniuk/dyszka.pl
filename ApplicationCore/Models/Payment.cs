using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Models.Interfaces;

namespace ApplicationCore.Models
{
    public class Payment : BaseEntity, ICreatedDate
    {
        public DateTime CreatedDate { get; set; }
        public Order Order { get; set; }
        public BillingData BillingData { get; set; }
        public decimal Value { get; set; }
        public bool Done { get; set; }
        public DateTime DoneTime { get; set; }
        public ApplicationUser DoneBy { get; set; }
    }
}
