using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Touba.WebApis.API.Services.Exceptions
{
    public class NotificationException : Exception
    {
        public NotificationException()
        {
        }

        public NotificationException(string message) : base(message)
        {
        }

        public NotificationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotificationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
