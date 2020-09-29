using System;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.ViewModels;

namespace ApplicationCore.Services
{
    public interface IPaymentService
    {
        public PagedResult<PaymentVm> GetPaged(int page);
        public Task AddPayment(Order order);
        public Task MarkAsDone(Guid id);
    }
}
