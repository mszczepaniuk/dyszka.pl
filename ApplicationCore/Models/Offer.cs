using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Models.Interfaces;

namespace ApplicationCore.Models
{
    public class Offer : AuditableEntity
    {
        public string Title { get; set; }
        public IList<string> Tags { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
