using System.Security.Claims;

namespace Touba.WebApis.API.Models.Account
{
    public class GetAllUserClaimsResponse
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
