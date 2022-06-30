using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Touba.WebApis.IdentityServer.DataLayer.Models
{
    public class Email
    {
        [Key]
        [MaxLength(100)]
        public string Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string EmailReceiver { get; set; }

        [Required]
        [MaxLength(100)]
        public string Subject { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(MAX)")]
        public string Body { get; set; }

        [Required]
        [MaxLength(100)]
        public string EmailSender { get; set; }

        [Required]
        [MaxLength(100)]
        public string Smtp { get; set; }

        [Required]
        public int Port { get; set; }

        [Required]
        public bool IsBodyHtml { get; set; }

        [Required]
        public bool EnableSsl { get; set; }

        [Required]
        public bool IsSend { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
