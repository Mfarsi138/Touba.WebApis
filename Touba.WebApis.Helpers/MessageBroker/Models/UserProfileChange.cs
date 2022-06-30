using System;

namespace Touba.WebApis.Helpers.MessageBroker.Models
{
    public class MB_UserProfileChange
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MiddleName { get; set; }

        public string UserId { get; set; }

        public string Gender { get; set; }

        public DateTime? BirthDate { get; set; }
    }
}
