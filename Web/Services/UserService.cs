using ApplicationCore.Models;
using ApplicationCore.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class UserService : BaseService<ApplicationUser>, IUserService
    {
        public string CurrentUserToken { get; set; }
        public ApplicationUser CurrentUser { get; set; }
        private readonly HttpClient client;
        private readonly IAuditLogService auditLogService;
        private readonly IBaseRepository<Offer> offerRepository;
        private readonly IBaseRepository<Comment> commentRepository;
        private readonly IBaseRepository<Message> messageRepository;
        private readonly IBaseRepository<Order> orderRepository;
        private readonly IBaseRepository<BillingData> billingDataRepository;
        private readonly IBaseRepository<Payment> paymentRepository;
        private readonly string identityUrl;
        private readonly string baseUrl;

        public UserService(IBaseRepository<ApplicationUser> repository,
            HttpClient client,
            IConfiguration config,
            IAuditLogService auditLogService,
            IBaseRepository<Offer> offerRepository,
            IBaseRepository<Comment> commentRepository,
            IBaseRepository<Message> messageRepository,
            IBaseRepository<Order> orderRepository,
            IBaseRepository<BillingData> billingDataRepository,
            IBaseRepository<Payment> paymentRepository) : base(repository)
            
        {
            this.client = client;
            this.auditLogService = auditLogService;
            this.offerRepository = offerRepository;
            this.commentRepository = commentRepository;
            this.messageRepository = messageRepository;
            this.orderRepository = orderRepository;
            this.billingDataRepository = billingDataRepository;
            this.paymentRepository = paymentRepository;
            baseUrl = config.GetSection("URI").GetValue<string>("IdentityServer");
            identityUrl = baseUrl + "/api/identity/";
        }

        public ApplicationUser GetByUserName(string username)
        {
            return repository.GetAll().AsNoTracking().FirstOrDefault(u => u.UserName == username);
        }

        public async Task<string> GetUserIdentityData(string username)
        {
            var response = await SendRequestWithToken(HttpMethod.Get, new Uri($"{identityUrl}{username}"));
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }
            return await response.Content.ReadAsStringAsync();
        }

        public async Task<IList<ApplicationUser>> GetAllInRole(string roleName)
        {
            var response = await SendRequestWithToken(HttpMethod.Get, new Uri($"{identityUrl}role/{roleName}"));
            if (!response.IsSuccessStatusCode)
            {
                return new List<ApplicationUser>();
            }
            var names = JsonSerializer.Deserialize<List<string>>(await response.Content.ReadAsStringAsync());
            var users = repository.GetAll().Where(u => names.Contains(u.UserName)).ToList();
            return users;
        }

        public async Task<bool> AddToRole(string username, string roleName)
        {
            var result = (await SendRequestWithToken(HttpMethod.Post, new Uri($"{identityUrl}claim/{username}/{roleName}"))).IsSuccessStatusCode;
            if (result)
            {
                await auditLogService.AddAuditLogAsync($"Dodano do roli: {roleName}", GetByUserName(username).Id);
            }
            return result;
        }

        public async Task<bool> RemoveFromRole(string username, string roleName)
        {
            var result = (await SendRequestWithToken(HttpMethod.Delete, new Uri($"{identityUrl}claim/{username}/{roleName}")))
                .IsSuccessStatusCode;
            if (result)
            {
                await auditLogService.AddAuditLogAsync($"Usunięto z roli: {roleName}", GetByUserName(username).Id);
            }
            return result;
        }

        public async Task<bool> BanUser(string username)
        {
            var result = (await SendRequestWithToken(HttpMethod.Post, new Uri($"{identityUrl}ban/{username}")))
                .IsSuccessStatusCode;
            if (result)
            {
                var offers = GetAll()
                    .Where(u => u.UserName == username)
                    .Include(u => u.Offers)
                    .SelectMany(u => u.Offers)
                    .ToList();
                foreach (var offer in offers)
                {
                    offer.IsBlocked = true;
                    await offerRepository.UpdateAsync(offer.Id, offer);
                }
                await auditLogService.AddAuditLogAsync("Zbanowano użytkownika", GetByUserName(username).Id);
            }
            return result;
        }

        public async Task<bool> UnbanUser(string username)
        {
            var result = (await SendRequestWithToken(HttpMethod.Post, new Uri($"{identityUrl}unban/{username}")))
                .IsSuccessStatusCode;
            if (result)
            {
                var offers = GetAll()
                    .Where(u => u.UserName == username)
                    .Include(u => u.Offers)
                    .SelectMany(u => u.Offers)
                    .ToList();
                foreach (var offer in offers)
                {
                    offer.IsBlocked = false;
                    await offerRepository.UpdateAsync(offer.Id, offer);
                }
                await auditLogService.AddAuditLogAsync("Odbanowano użytkownika", GetByUserName(username).Id);
            }
            return result;
        }

        public BillingData GetUserBillingData(string username)
        {
            return billingDataRepository.GetAll().Where(b => b.CreatedBy.UserName == username).AsNoTracking().FirstOrDefault();
        }

        public async Task CreateOrUpdateUserBillingData(string username, BillingData billingData)
        {
            var billingDataFromDb = GetUserBillingData(username);
            if (billingDataFromDb == null)
            {
                await billingDataRepository.AddAsync(billingData);
            }
            else
            {
                await billingDataRepository.UpdateAsync(billingDataFromDb.Id, billingData);
            }
        }

        public override async Task<bool> RemoveAsync(Guid id)
        {
            var appUser = repository.GetAll().AsNoTracking().FirstOrDefault(u => u.Id == id);
            if (appUser == null)
            {
                return false;
            }
            var response = await SendRequestWithToken(HttpMethod.Delete, new Uri($"{identityUrl}{appUser.UserName}"));
            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            await ResolveRemovalDependencies(id);

            if (appUser.UserName != CurrentUser.UserName)
            {
                await auditLogService.AddAuditLogAsync("Usunięto konto użytkownika", appUser.Id);
            }
            await repository.RemoveAsync(id);
            return true;
        }

        public override Task<ApplicationUser> UpdateAsync(Guid id, ApplicationUser item)
        {
            item.UserName = GetAll().AsNoTracking().Where(u => u.Id == id).Select(u => u.UserName).FirstOrDefault();
            return base.UpdateAsync(id, item);
        }

        private async Task<HttpResponseMessage> SendRequestWithToken(HttpMethod method, Uri uri)
        {
            var request = new HttpRequestMessage(method, uri);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", CurrentUserToken);
            var response = await client.SendAsync(request);
            request.Dispose();
            return response;
        }

        private async Task ResolveRemovalDependencies(Guid id)
        {
            foreach (var comment in commentRepository.GetAll().Where(c => c.CreatedBy.Id == id).ToList())
            {
                await commentRepository.RemoveAsync(comment.Id);
            }

            foreach (var message in messageRepository.GetAll().Where(m => m.CreatedBy.Id == id || m.Receiver.Id == id).ToList())
            {
                await messageRepository.RemoveAsync(message.Id);
            }

            foreach (var offer in offerRepository.GetAll().Where(o => o.CreatedBy.Id == id).ToList())
            {
                offer.IsHidden = true;
                offer.CreatedBy = null;
                offer.UpdatedBy = null;
                await offerRepository.UpdateAsync(offer.Id, offer);
            }

            foreach (var order in orderRepository.GetAll().Where(o => o.CreatedBy.Id == id).ToList())
            {
                order.CreatedBy = null;
                await orderRepository.UpdateAsync(order.Id, order);
            }

            foreach (var billingData in billingDataRepository.GetAll().Where(o => o.CreatedBy.Id == id).ToList())
            {
                billingData.CreatedBy = null;
                await billingDataRepository.UpdateAsync(billingData.Id, billingData);
            }

            foreach (var payment in paymentRepository.GetAll().Where(p => p.DoneBy.Id == id).ToList())
            {
                payment.DoneBy = null;
                await paymentRepository.UpdateAsync(payment.Id, payment);
            }
        }
    }
}
