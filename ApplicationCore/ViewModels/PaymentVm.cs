using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.ViewModels
{
    public class PaymentVm
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid OrderId { get; set; }
        public Guid OfferId { get; set; }
        public string ReceiverUserName { get; set; }
        public decimal Value { get; set; }
    }
}
