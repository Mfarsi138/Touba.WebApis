using System.ComponentModel.DataAnnotations;

namespace Touba.WebApis.API.Models.Account
{
    public class NewClaimRequest
    {
        [Required]
        public string UserName { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
