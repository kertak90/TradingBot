using System;
using System.Collections.Generic;
using System.Text;

namespace TradingCore.Sundries
{
    public static class DateTimeExtensionMethods
    {   
        public static DateTime TrimMilliseconds(this DateTime dt)
        {
            return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second);
        }
    }
}
