using Touba.WebApis.API.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Touba.WebApis.API.Models.Account
{
    public class LoginResponse : ResponseModel
    {
        public string DisplayName { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpireDate { get; set; }
        public string TokenType { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
        public bool PhoneNumberNotConfirmed { get; set; }
        public string PhoneNumber { get; set; }

        public bool IsConfirmed { get; set; }

        public string State { get; set; }

        public bool IsDraft { get; set; }

        public string MainObjectId { get; set; }
    }
}
