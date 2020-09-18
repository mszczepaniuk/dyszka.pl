using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.ViewModels;

namespace Web.Services.Interfaces
{
    public interface IMessageService : IExtendedBaseService<Message>
    {
        public PagedResult<MessageVm> GetPagedAndFiltered(int page, string user1, string user2);
        public PagedResult<MessageVm> GetLastReceivedMessages(int page, string user);
    }
}
