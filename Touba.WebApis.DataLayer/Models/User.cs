using Touba.WebApis.DataLayer.Enums;
using Microsoft.AspNetCore.Identity;
using System;

namespace Touba.WebApis.IdentityServer.DataLayer.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public Gender Gender { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool IsActive { get; set; }

        public long? MainObjectId { get; set; }
    }
}
