using System;

namespace Touba.WebApis.Helpers.MessageBroker.Models
{
    public class AttachmentSaved : IBaseMessage
    {
        public string FileName { get; set; }

        public Guid MessageId { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
