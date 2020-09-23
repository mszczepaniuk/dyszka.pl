using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.BindingModels
{
    public class OfferBm
    {
        [Required]
        public string Image { get; set; }
        [Required]
        [MaxLength(30)]
        public string Title { get; set; }
        [Required]
        public IList<string> Tags { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        [MaxLength(160)]
        public string ShortDescription { get; set; }
        [Required]
        [Range(10, double.PositiveInfinity)]
        public decimal Price { get; set; }
    }
}
