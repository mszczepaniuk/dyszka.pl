using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.BindingModels
{
    public class OfferBm
    {
        public string Image { get; set; }
        public string Title { get; set; }
        public IList<string> Tags { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public decimal Price { get; set; }
    }
}
