using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Models;
using ApplicationCore.Models.Options;
using ApplicationCore.Repositories;
using ApplicationCore.Services;
using ApplicationCore.ViewModels;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Web.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBaseRepository<Payment> paymentRepository;
        private readonly IMapper mapper;
        private readonly IOptions<PaymentOptions> paymentOptions;
        private readonly IUserService userService;
        private int ResultsPerPage => 10;

        public PaymentService(IBaseRepository<Payment> paymentRepository,
            IMapper mapper,
            IOptions<PaymentOptions> paymentOptions,
            IUserService userService)
        {
            this.paymentRepository = paymentRepository;
            this.mapper = mapper;
            this.paymentOptions = paymentOptions;
            this.userService = userService;
        }

        public PagedResult<PaymentVm> GetPaged(int page)
        {
            var query = paymentRepository.GetAll()
                .OrderBy(p => p.CreatedDate)
                .Where(p => !p.Done)
                .Include(p => p.BillingData)
                .Include(p => p.Order)
                .ThenInclude(o => o.Offer)
                .ThenInclude(o => o.CreatedBy);
            return new PagedResult<PaymentVm>
            {
                Items = mapper.Map<List<PaymentVm>>(query.Skip((page - 1) * ResultsPerPage).Take(ResultsPerPage).ToList()),
                CurrentPage = page,
                ResultsPerPage = ResultsPerPage,
                PagesCount = query.Any() ?
                    (query.Count() - 1) / ResultsPerPage + 1 :
                    0
            };
        }

        public async Task AddPayment(Order order)
        {
            await paymentRepository.AddAsync(new Payment
            {
                Done = false,
                Value = (decimal)paymentOptions.Value.AppMargin / 100 * order.Offer.Price,
                Order = order,
                BillingData = userService.GetUserBillingData(order.Offer.CreatedBy.UserName)
            });
        }

        public async Task MarkAsDone(Guid id)
        {
            var payment = paymentRepository.GetById(id);
            payment.Done = true;
            payment.DoneBy = userService.GetById(userService.CurrentUser.Id);
            await paymentRepository.UpdateAsync(id, payment);
        }
    }
}
