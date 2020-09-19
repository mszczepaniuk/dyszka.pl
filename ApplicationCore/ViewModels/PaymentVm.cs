using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ViewModels
{
    public class PaymentVm
    {
        public DateTime CreatedDate { get; set; }
        public Guid OrderId { get; set; }
        public Guid OfferId { get; set; }
        public Guid BillingDataId { get; set; }
    }
}
