
using Touba.WebApis.API.Models.Enums;

namespace Touba.WebApis.IdentityServer.API.Models.Email
{
    public class EmailReceiverSettings
    {
        public Entry[] Servers { get; set; }

        public int NumberOfEmailsReadInEachIteration { get; set; }

        public class Entry
        {
            public string ServerAddress { get; set; }

            public int Port { get; set; }


            public string Username { get; set; }

            public string Password { get; set; }

            public MailReceiverProtocol Protocol { get; set; }
        }
    }
}