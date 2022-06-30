using System;

namespace Touba.WebApis.Helpers.MessageBroker.Models
{
    public interface IBaseMessage
    {
        string FileName { get; set; }

        Guid MessageId { get; set; }

        DateTime CreationDate { get; set; }
    }
}
