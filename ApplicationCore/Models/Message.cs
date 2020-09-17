using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Models.Interfaces;

namespace ApplicationCore.Models
{
    public class Message : BaseEntity, IOwnable, ICreatedDate
    {
        public ApplicationUser CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public ApplicationUser Receiver { get; set; }
        public string Text { get; set; }
    }
}
