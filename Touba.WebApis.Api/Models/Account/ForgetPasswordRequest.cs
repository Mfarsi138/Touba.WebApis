using System.ComponentModel.DataAnnotations;

namespace Touba.WebApis.API.Models.Account
{
    public class ForgetPasswordRequest
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "-4500")]
        public string Email { get; set; }
        [MaxLength(256)]
        public string Username { get; set; }

    }
}
