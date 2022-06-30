using System;
using System.Collections.Generic;
using System.Text;

namespace Touba.WebApis.Helpers.MessageBroker.Models.Account
{
    public class MB_GetUserInfoByIdRequest
    {
        public string UserId { get; set; }

    }
    public class MB_GetUserInfoByIdResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsTemporaryCustomer { get; set; }
        public string ProfileImageUrl { get; set; }
        public bool IsActive { get; set; }
        public long? MainObjectId { get; set; }

    }


}
