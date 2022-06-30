using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Touba.WebApis.API.Models
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; set; }

        public string AllowedHosts { get; set; }

        public Serilog Serilog { get; set; }

        public Security Security { get; set; }

        public QueueSetting QueueSetting { get; set; }
        
        public string IdentityServerAddress { get; set; }

        public List<Client> Clients { get; set; }

        public bool EnableCORS { get; set; }

        public string[] CORSOrigins { get; set; }
       // public Elk Elk { get; set; }
        //public EmailConfiguration EmailConfiguration { get; set; }
        public MessageConfiguration MessageConfiguration { get; set; }
        public string APIGatewayBaseAddress { get; set; }
        public string ResetPasswordUrl { get; set; }
        
    }

    public class ConnectionStrings
    {
        public string MSSQL { get; set; }
    }

    public class Override
    {
        public string Microsoft { get; set; }
        public string System { get; set; }
    }

    public class MinimumLevel
    {
        public string Default { get; set; }
        public Override Override { get; set; }
    }

    public class Serilog
    {
        public MinimumLevel MinimumLevel { get; set; }
        public List<string> Enrich { get; set; }
        public string LogFileName { get; set; }
    }

    public class Security
    {
        public string EncDecHelperKey { get; set; }

        public string TokenSecret { get; set; }
    }

    public class QueueSetting
    {
        public string HostName { get; set; }

        public string VirtualHost { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string QueueName { get; set; }
    }

    public class Client
    {
        public string ClientName { get; set; }

        public string ClientId { get; set; }

        public bool Enabled { get; set; }

        public List<string> RedirectUris { get; set; }

        public List<string> AllowedCorsOrigins { get; set; }

        public List<string> PostLogoutRedirectUris { get; set; }
    }
    //public class Elk
    //{
    //    public string Elasticsearch { get; set; }
    //    public string Kibana { get; set; }
    //    public string Logstash { get; set; }
    //}
    //public class EmailConfiguration
    //{
    //    public string EmailSender { get; set; }
    //    public string Password { get; set; }
    //    public string Smtp { get; set; }
    //    public int Port { get; set; }
    //    public bool IsBodyHtml { get; set; }
    //    public bool EnableSsl { get; set; }
    //}
    public class MessageConfiguration
    {
        public string url { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string sender { get; set; }
    }
}
