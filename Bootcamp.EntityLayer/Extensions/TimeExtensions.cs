using System;

namespace Bootcamp.EntityLayer.Extensions
{
    public static class TimeExtensions
    {
        public static string FormatDuration(this int seconds)
        {
            if (seconds <= 0)
                return "0:00";

            var timeSpan = TimeSpan.FromSeconds(seconds);
            
            if (timeSpan.TotalHours >= 1)
            {
                return $"{(int)timeSpan.TotalHours}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            }
            else
            {
                return $"{timeSpan.Minutes}:{timeSpan.Seconds:D2}";
            }
        }
    }
} 