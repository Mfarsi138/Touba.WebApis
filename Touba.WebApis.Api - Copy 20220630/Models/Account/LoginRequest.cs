using System.ComponentModel.DataAnnotations;

namespace Touba.WebApis.API.Models.Account
{
    public class LoginRequest
    {
        [MaxLength(100)]
        public string Username { get; set; }
        [MaxLength(100)]
        public string Password { get; set; }
        //[MaxLength(100)]
        //public string ClientId { get; set; }
        public bool RememberLogin { get; set; }
    }
}
