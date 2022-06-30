using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Touba.WebApis.IdentityServer.API.Models.Email
{
    public class EmailSendModel
    {
        [Required(ErrorMessage = "The email receiver is not defined.")]
        public string EmailReceiver { get; set; }

        [Required(ErrorMessage = "The email subject is not defined.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "The email body is not defined.")]
        public string Body { get; set; }

        public bool IsSend { get; set; }
    }
}
