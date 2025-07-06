using System;
using TimeZoneConverter;

namespace home_manager.Helpers
{
    public static class TimeZoneHelper
    {
        private static string timeZoneId = "America/Phoenix";
        private static readonly TimeZoneInfo LocalTimeZone = TZConvert.GetTimeZoneInfo(timeZoneId);

        public static DateTime LocalTime => TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, LocalTimeZone);
    }
}
