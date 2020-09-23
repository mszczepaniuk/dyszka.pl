using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Models.Interfaces;

namespace ApplicationCore.Models
{
    public class Offer : AuditableEntity
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public IList<string> Tags { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsHidden { get; set; }
        public IList<Comment> Comments { get; set; }
    }
}
