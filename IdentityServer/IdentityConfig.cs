using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;

namespace IdentityServer
{
    public static class IdentityConfig
    {
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("web.all", "dyszka.pl webApi"),
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName, "Identity Server local endpoints")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "web",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secretWeb".Sha256())
                    },
                    AllowedScopes = { "web.all", "offline_access", IdentityServerConstants.LocalApi.ScopeName },
                    AllowedCorsOrigins = { "https://localhost:5001" },
                    AccessTokenLifetime = 60 * 30,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    UpdateAccessTokenClaimsOnRefresh = true
                },
                new Client
                {
                    ClientId = "mobile",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secretMobile".Sha256())
                    },
                    AllowedScopes = { "web.all", "offline_access", IdentityServerConstants.LocalApi.ScopeName },
                    AccessTokenLifetime = 60 * 60 * 24 * 365,
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly,
                    UpdateAccessTokenClaimsOnRefresh = true
                }
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("web", "dyszka.pl webAPI")
                {
                    Scopes = { "web.all" }
                },
                new ApiResource(IdentityServerConstants.LocalApi.ScopeName, "Identity Server local endpoints")
                {
                    Scopes = { IdentityServerConstants.LocalApi.ScopeName }
                }
            };
        }
    }
}
