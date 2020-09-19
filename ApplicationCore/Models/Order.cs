using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Models.Interfaces;

namespace ApplicationCore.Models
{
    public class Order : BaseEntity, ICreatedDate, IOwnable
    {
        public DateTime CreatedDate { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public Offer Offer { get; set; }
        public bool Done { get; set; }
        public DateTime? DoneTime { get; set; }
    }
}
