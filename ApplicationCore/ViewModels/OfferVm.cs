using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ViewModels
{
    public class OfferVm
    {
        public Guid Id { get; set; }
        public string AuthorUserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public IList<string> Tags { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
        public bool IsBlocked { get; set; }
        public bool IsHidden { get; set; }
    }
}
