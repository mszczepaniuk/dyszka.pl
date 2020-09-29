using ApplicationCore.Models;
using ApplicationCore.ViewModels;

namespace ApplicationCore.Services
{
    public interface IMessageService : IExtendedBaseService<Message>
    {
        public PagedResult<MessageVm> GetPagedAndFiltered(int page, string user1, string user2);
        public PagedResult<MessageVm> GetLastReceivedMessages(int page, string user);
    }
}
