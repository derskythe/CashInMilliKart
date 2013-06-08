using System;

namespace Containers
{
    public struct DateTimeWithZone
    {
        private readonly DateTime _UtcDateTime;
        private readonly TimeZoneInfo _TimeZone;

        public DateTimeWithZone(DateTime dateTime, TimeZoneInfo timeZone)
        {
            _UtcDateTime = TimeZoneInfo.ConvertTimeToUtc(dateTime, timeZone);
            _TimeZone = timeZone;
        }

        public DateTime UniversalTime
        {
            get { return _UtcDateTime; }
        }

        public TimeZoneInfo TimeZone
        {
            get { return _TimeZone; }
        }

        public DateTime LocalTime
        {
            get
            {
                return TimeZoneInfo.ConvertTime(_UtcDateTime, _TimeZone);
            }
        }
    }
}
