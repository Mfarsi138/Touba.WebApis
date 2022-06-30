﻿using System.ComponentModel.DataAnnotations;

namespace Touba.WebApis.API.Models.Account
{
    public class ChangeEmailRequest
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "-4500")]
        public string Email { get; set; }
    }
}
