using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Repositories;
using ApplicationCore.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseRepository<Order> orderRepository;
        private readonly IMapper mapper;
        private readonly IUserService userService;
        private readonly IBaseRepository<Offer> offerRepository;
        private int ResultsPerPage => 10;

        public OrderService(IBaseRepository<Order> orderRepository,
            IMapper mapper,
            IUserService userService,
            IBaseRepository<Offer> offerRepository)
        {
            this.orderRepository = orderRepository;
            this.mapper = mapper;
            this.userService = userService;
            this.offerRepository = offerRepository;
        }

        public Order GetByIdAsNoTracking(Guid id)
        {
            return orderRepository.GetAll().Where(o => o.Id == id).Include(o => o.CreatedBy).AsNoTracking().FirstOrDefault();
        }

        public PagedResult<OrderVm> GetCreatedByCurrentUser(int page)
        {
            var query = orderRepository.GetAll().Where(o => o.CreatedBy == userService.CurrentUser && !o.Done)
                .Include(o => o.CreatedBy)
                .Include(o => o.Offer)
                .ThenInclude(o => o.CreatedBy);
            return new PagedResult<OrderVm>
            {
                Items = mapper.Map<List<OrderVm>>(query.Skip((page - 1) * ResultsPerPage).Take(ResultsPerPage).ToList()),
                CurrentPage = page,
                ResultsPerPage = ResultsPerPage,
                PagesCount = query.Any() ?
                    (query.Count() - 1) / ResultsPerPage + 1 :
                    0
            };
        }

        public PagedResult<OrderVm> GetOrdersForCurrentUserOffers(int page, bool done)
        {
            var query = orderRepository.GetAll().Where(o => o.Offer.CreatedBy == userService.CurrentUser && o.Done == done)
                .Include(o => o.CreatedBy)
                .Include(o => o.Offer)
                .ThenInclude(o => o.CreatedBy); ;
            return new PagedResult<OrderVm>
            {
                Items = mapper.Map<List<OrderVm>>(query.Skip((page - 1) * ResultsPerPage).Take(ResultsPerPage).ToList()),
                CurrentPage = page,
                ResultsPerPage = ResultsPerPage,
                PagesCount = query.Any() ?
                    (query.Count() - 1) / ResultsPerPage + 1 :
                    0
            };
        }

        public async Task CreateOrder(Guid offerId)
        {
            await orderRepository.AddAsync(new Order
            {
                Done = false,
                Offer = offerRepository.GetById(offerId)
            });
        }

        public async Task MarkAsDone(Guid orderId)
        {
            // CREATE PAYMENT HERE
            var order = orderRepository.GetById(orderId);
            order.Done = true;
            order.DoneTime = DateTime.Now;
            await orderRepository.UpdateAsync(orderId, order);
        }
    }
}
