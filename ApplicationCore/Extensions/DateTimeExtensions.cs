using System;
using System.Collections.Generic;
using System.Text;

namespace ApplicationCore.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool IsEarlier(this DateTime thisDate, DateTime date)
        {
            return thisDate.CompareTo(date) < 0;
        }

        public static bool IsLater(this DateTime thisDate, DateTime date)
        {
            return thisDate.CompareTo(date) > 0;
        }
    }
}
