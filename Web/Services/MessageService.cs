using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using ApplicationCore.Services;
using ApplicationCore.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Web.Services
{
    public class MessageService : ExtendedBaseService<Message>, IMessageService
    {
        protected override int ResultsPerPage => 20;

        public MessageService(IBaseRepository<Message> repository, IMapper mapper) : base(repository, mapper)
        {
        }

        public PagedResult<MessageVm> GetPagedAndFiltered(int page, string user1, string user2)
        {
            var query = repository.GetAll().OrderByDescending(m => m.CreatedDate)
                .Where(m => (m.CreatedBy.UserName == user1 && m.Receiver.UserName == user2) || (m.CreatedBy.UserName == user2 && m.Receiver.UserName == user1))
                .Include(m => m.CreatedBy).Include(m => m.Receiver);

            return new PagedResult<MessageVm>
            {
                Items = mapper.Map<List<MessageVm>>(query.Skip((page - 1) * ResultsPerPage).Take(ResultsPerPage).ToList()),
                CurrentPage = page,
                ResultsPerPage = ResultsPerPage,
                PagesCount = query.Any() ?
                    (query.Count() - 1) / ResultsPerPage + 1 :
                    0
            };
        }

        public PagedResult<MessageVm> GetLastReceivedMessages(int page, string user)
        {
            var query = repository.GetAll().OrderByDescending(m => m.CreatedDate)
                .Where(m => m.Receiver.UserName == user)
                .Include(m => m.CreatedBy)
                .Include(m => m.Receiver)
                .ToList()
                .GroupBy(m => m.CreatedBy)
                .Select(g => g.First());

            return new PagedResult<MessageVm>
            {
                Items = mapper.Map<List<MessageVm>>(query.Skip((page - 1) * ResultsPerPage).Take(ResultsPerPage).ToList()),
                CurrentPage = page,
                ResultsPerPage = ResultsPerPage,
                PagesCount = query.Any() ?
                    (query.Count() - 1) / ResultsPerPage + 1 :
                    0
            };
        }
    }
}
