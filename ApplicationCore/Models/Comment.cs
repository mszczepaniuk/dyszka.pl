using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Models
{
    public class Comment : AuditableEntity
    {
        public string Text { get; set; }
        public bool IsPositive  { get; set; }
        public Offer Offer { get; set; }
    }
}
