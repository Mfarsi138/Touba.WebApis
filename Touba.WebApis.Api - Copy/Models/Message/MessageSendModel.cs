using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Touba.WebApis.IdentityServer.API.Models.Message
{
    public class MessageSendModel
    {
        public string Receiver { get; set; }
        public string Body { get; set; }
        public string UniCode { get; set; }
    }
}
