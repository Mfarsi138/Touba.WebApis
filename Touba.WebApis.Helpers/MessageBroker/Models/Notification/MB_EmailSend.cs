using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Touba.WebApis.Helpers.MessageBroker.Models.Notification
{
    public class MB_EmailSend
    {
        [Required(ErrorMessage = "The email receiver is not defined.")]
        public string EmailReceiver { get; set; }

        [Required(ErrorMessage = "The email subject is not defined.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "The email body is not defined.")]
        public string Body { get; set; }

        public bool IsSend { get; set; }
    }
    public class MB_SmsSend
    { 
     public string Receiver { get; set; }
     public string Body { get; set; }
     public string UniCode { get; set; }



    }
}
