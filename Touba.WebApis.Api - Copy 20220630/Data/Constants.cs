using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Touba.WebApis.API.Data
{
    public class Constants
    {
        public static string UsernameIsAlreadyExists = "-1500";
        public const string ArabicLettresRegex = @"^[\u0621-\u063A\u0641-\u0652\s]*$";
        public const string EnglishLettresRegex = @"^[A-Za-z ]+$";
    }
}
