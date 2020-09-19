using System;
using System.Collections.Generic;
using System.Text;
using ApplicationCore.Models.Interfaces;

namespace ApplicationCore.ViewModels
{
    public class OrderVm
    {
        public Guid OfferId { get; set; }
        public string OfferAuthorUserName { get; set; }
        public string OfferTitle { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? DoneTime { get; set; }
        public string AuthorUserName { get; set; }
    }
}
