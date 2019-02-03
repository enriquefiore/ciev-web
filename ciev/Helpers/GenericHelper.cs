using System;
namespace ciev.Helpers
{
    public class GenericHelper
    {
        public DateTime ConvertTimezone(DateTime input) 
        {
            var zone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            return TimeZoneInfo.ConvertTime(input, zone);
        }
    }
}
