using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Touba.WebApis.IdentityServer.DataLayer.Models
{
    public class Message
    {
        [Key]
        [MaxLength(100)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Sender { get; set; }

        [Required]
        [MaxLength(100)]
        public string Receiver { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Body { get; set; }

        [Required]
        [MaxLength(100)]
        public string Command { get; set; }

        [Required]
        public bool IsSend { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
