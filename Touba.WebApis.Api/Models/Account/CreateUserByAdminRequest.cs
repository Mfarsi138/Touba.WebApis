using System.ComponentModel.DataAnnotations;

namespace Touba.WebApis.API.Models.Account
{
    public class CreateUserByAdminRequest
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Password { get; set; }
        public string[] Roles { get; set; }
        public string PhoneNumber { get; set; }
    }
}
