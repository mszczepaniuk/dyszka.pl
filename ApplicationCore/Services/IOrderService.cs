using System;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.ViewModels;

namespace ApplicationCore.Services
{
    public interface IOrderService
    {
        public Order GetByIdAsNoTracking(Guid id);
        public PagedResult<OrderVm> GetCreatedByCurrentUser(int page);
        public PagedResult<OrderVm> GetOrdersForCurrentUserOffers(int page, bool done);
        public Task CreateOrder(Guid offerId);
        public Task MarkAsDone(Guid orderId);
    }
}
