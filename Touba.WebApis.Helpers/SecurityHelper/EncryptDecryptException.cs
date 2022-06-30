using System;

namespace Touba.WebApis.Helpers.SecurityHelper
{
    public class EncryptDecryptException : Exception
    {
        public EncryptDecryptException()
        {

        }

        public EncryptDecryptException(string message) : base(message)
        {
        }

        public EncryptDecryptException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
