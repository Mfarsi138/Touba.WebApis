using System;
using System.Globalization;

namespace Touba.WebApis.Helpers.Helpers
{
    public class DateTimeHelper
    {
        public static DateTime? StringToDateTime(string dateTime)
        {
            DateTime parsedDateTime;
            if (DateTime.TryParse(dateTime, out parsedDateTime))
                return parsedDateTime;
            else
                return null;
        }
        public static DateTime StringToDateTime2(string dateTime)
        {
            DateTime parsedDateTime;
            if (DateTime.TryParse(dateTime, out parsedDateTime))
                return parsedDateTime;
            else
                return DateTime.Now;
        }
        public static DateTime CreateDateTime(DateTime dateTime, TimeSpan time)
        {
            return new DateTime(
                dateTime.Year, dateTime.Month, dateTime.Day,
                time.Hours, time.Minutes, time.Seconds);
        }
        public static DateTime CreateTruncatedDateTime(DateTime dateTime, int replaceHourWith)
        {
            return new DateTime(
                dateTime.Year, dateTime.Month, dateTime.Day,
                replaceHourWith, 0, 0);
        }
        public static DateTime PersianDateStringToDateTime(string _PersianDateString)
        {
            DateTime _DT = default(DateTime);
            if (!string.IsNullOrEmpty(_PersianDateString))
            {
                string[] _DateParts = _PersianDateString.Split('/');
                int _Parse = 0;
                if (_DateParts.Length == 3 && int.TryParse(_DateParts[0], out _Parse) && int.TryParse(_DateParts[1], out _Parse) && int.TryParse(_DateParts[2], out _Parse))
                {
                    try
                    {
                        PersianCalendar _pc = new PersianCalendar();
                        _DT = (new PersianCalendar()).ToDateTime(Convert.ToInt32(_DateParts[0]), Convert.ToInt32(_DateParts[1]), Convert.ToInt32(_DateParts[2]), 0, 0, 0, 0);
                    }
                    catch { }
                }

            }
            return _DT;
        }
        public static string DateTimeToPersianDateString(DateTime _DateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return $"{pc.GetYear(_DateTime)}/{pc.GetMonth(_DateTime)}/{pc.GetDayOfMonth(_DateTime)}";
        }
        public static bool FromNowOn(DateTime _DateTime) => _DateTime > DateTime.Now;
        public static bool BeforeNow(DateTime _DateTime) => _DateTime < DateTime.Now;
    }
}
