using System;
using System.ComponentModel.DataAnnotations;

namespace Touba.WebApis.IdentityServer.DataLayer.Models
{
    public class MailReceiverLogEntity
    {
        [Key] public Guid Id { get; set; }

        //Message's uniqueId in third-party email provider.
        public Guid MessageUniqueId { get; set; }

        
        public DateTimeOffset CreatedDateTime { get; set; }
    }
}
