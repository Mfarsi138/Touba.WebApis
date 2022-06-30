using System.ComponentModel.DataAnnotations;

namespace Touba.WebApis.API.Models.Account
{
    public class ConfirmPhoneNumberRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Token { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }
    }
}
