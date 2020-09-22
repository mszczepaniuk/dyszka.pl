﻿using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Web.Authorization;

namespace Web.IntegrationTests.Configuration
{
    public class FakeAdminAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public FakeAdminAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {

        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var claims = new List<Claim>
            {
                new Claim(AuthConstants.UserNameClaimType, "administrator"),
                new Claim(AuthConstants.IsBannedClaimType, "False"),
                new Claim(ClaimTypes.Role, AuthConstants.AdminRoleName),
                new Claim(ClaimTypes.Role, AuthConstants.ModeratorRoleName)
            };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            var result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
