using Touba.WebApis.API.Models;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Touba.WebApis.API.Data
{
    public class Clients
    {
        public static List<IdentityServer4.Models.Client> identityClients;

        public static List<IdentityServer4.Models.Client> GetClients(AppSettings appSettings)
        {
            var result = new List<IdentityServer4.Models.Client>();
            foreach(var client in appSettings.Clients)
            {
                result.Add(new IdentityServer4.Models.Client
                {
                    ClientName = client.ClientName,
                    ClientId = client.ClientId,
                    Enabled = client.Enabled,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    RedirectUris = client.RedirectUris,
                    AllowAccessTokensViaBrowser = true,
                    AllowedScopes =
                       {
                           IdentityServerConstants.StandardScopes.OpenId,
                           IdentityServerConstants.StandardScopes.OfflineAccess,
                           // IdentityServerConstants.StandardScopes.Profile,
                           "zakaApi",
                           "orphanApi",
                           "customerApi",
                           "dmsApi",
                           "allApis"
                       },
                    AllowedCorsOrigins = client.AllowedCorsOrigins,
                    AllowOfflineAccess = true,
                    RequireClientSecret = false,
                    PostLogoutRedirectUris = client.PostLogoutRedirectUris,
                    RequireConsent = false,
                    AccessTokenLifetime = 3600
                });
            }
            identityClients = result;
            return result;
        }
    }
}
