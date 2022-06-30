using Touba.WebApis.Helpers.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    public static class ExtensionMethods
    {
        public static string GetEnumDescription(this Enum value)
        {
            var type = value.GetType();
            var memInfo = type.GetMember(value.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Any())
                return ((DescriptionAttribute)attributes[0]).Description;
            else
                return value.ToString();
        }

        public static string Ellipsis(this string text, EllipsisPosition ellipsisPosition = EllipsisPosition.End, int maxLength = 50)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            if (text.Length <= maxLength)
                return text;

            switch (ellipsisPosition)
            {
                case EllipsisPosition.Start:
                    return string.Concat("...", text.Substring(text.Length - (maxLength + 3)));
                case EllipsisPosition.Middle:
                    return string.Concat(text.Substring(0, (maxLength - 3) / 2), "...", text.Substring(text.Length - (maxLength - 3) / 2));
                default:
                    return string.Concat(text.Substring(0, maxLength - 3), "...");
            }
        }

        public static bool IsValidEmailAddress(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            var regex = new Regex("^(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])$");
            return regex.IsMatch(value);
        }

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self) => self.Select((item, index) => (item, index)); 
    }
}
