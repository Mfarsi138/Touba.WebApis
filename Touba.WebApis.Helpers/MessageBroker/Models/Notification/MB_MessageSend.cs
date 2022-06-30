using System.ComponentModel.DataAnnotations;

namespace Touba.WebApis.Helpers.MessageBroker.Models.Notification
{
    public class MB_MessageSend
    {
        [Required(ErrorMessage = "The SMS receiver is not defined.")]
        public string Receiver { get; set; }

        [Required(ErrorMessage = "The SMS body is not defined.")]
        public string Body { get; set; }

        [Required(ErrorMessage = "The Unicode is not defined.")]
        public string UniCode { get; set; }

        public bool IsSent { get; set; }
    }
}
