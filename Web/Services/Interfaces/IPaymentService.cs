using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.ViewModels;

namespace Web.Services.Interfaces
{
    public interface IPaymentService
    {
        public PagedResult<PaymentVm> GetPaged(int page);
        public Task AddPayment(Order order);
        public Task MarkAsDone(Guid id);
    }
}
