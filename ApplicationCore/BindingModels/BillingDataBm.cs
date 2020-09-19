using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ApplicationCore.BindingModels
{
    public class BillingDataBm
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MaxLength(26)]
        [MinLength(26)]
        public string BankAccountNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }
}
