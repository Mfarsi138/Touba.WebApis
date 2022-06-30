using IdentityModel;
using IdentityServer4.Models;
using System.Collections.Generic;

namespace Touba.WebApis.API.Data
{
    public class IdentityResources
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
            new List<IdentityResource>
            {
                new IdentityServer4.Models.IdentityResources.Email(),
                new IdentityServer4.Models.IdentityResources.Phone(),
                new IdentityServer4.Models.IdentityResources.OpenId(),
                // new IdentityResource("roles", "User role(s)", new List<string> { "role" })
            };

        public static IEnumerable<ApiScope> GetApiScopes() =>
           new List<ApiScope>
           {
                new ApiScope("zakaApi", "Zaka module API"),
                new ApiScope("dmsApi", "Document Management System API"),
                new ApiScope("customerApi", "Customer Management System API"),
                new ApiScope("orphanApi", "Orphan management module API"),
                new ApiScope("allApis", "All APIs"),
                new ApiScope("seasonalApi", "All seasonal Apis")
           };

        public static IEnumerable<ApiResource> GetApiResources() =>
            new List<ApiResource>
            {
                new ApiResource("zakaApi", "Zaka module API", GetIdentityJwtClaims())
                {
                    Scopes = { "zakaApi" }
                },
                new ApiResource("orphanApi", "Orphan management module API", GetIdentityJwtClaims())
                {
                    Scopes = { "orphanApi" }
                },
                new ApiResource("customerApi", "Customer Management System API", GetIdentityJwtClaims())
                {
                    Scopes = { "customerApi" }
                },
                new ApiResource("allApis", "All APIs", GetIdentityJwtClaims())
                {
                    Scopes = { "allApis" }
                },
                new ApiResource("seasonalApi", "All APIs", GetIdentityJwtClaims())
                {
                    Scopes = { "seasonalApi" }
                }
            };

        /// <summary>
        /// Specifies the User Identity information that is returned in the JWT claims
        /// </summary>
        /// <returns>Array of the supported claim types</returns>
        public static IEnumerable<string> GetIdentityJwtClaims() =>
            new []
            {
                JwtClaimTypes.Role, 
                JwtClaimTypes.Email, 
                JwtClaimTypes.Name, 
                JwtClaimTypes.FamilyName, 
                JwtClaimTypes.MiddleName
            };

    }
}
