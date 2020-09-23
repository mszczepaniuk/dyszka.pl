using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Models
{
    public class BillingData : AuditableEntity
    {
        public string Name { get; set; }
        public string BankAccountNumber { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
    }
}
