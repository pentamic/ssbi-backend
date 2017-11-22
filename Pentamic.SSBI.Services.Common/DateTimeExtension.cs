using System;

namespace Pentamic.SSBI.Services.Common
{
    public static class DateTimeExtension
    {
        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }
        public static DateTime EndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }
        public static DateTime StartOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 1, 1);
        }
        public static DateTime EndOfYear(this DateTime date)
        {
            return new DateTime(date.Year, 12, 31);
        }
        public static DateTime StartOfQuarter(this DateTime date)
        {
            if (date.Month >= 1 && date.Month <= 3)
            {
                return new DateTime(date.Year, 1, 1);
            }
            if (date.Month >= 4 && date.Month <= 6)
            {
                return new DateTime(date.Year, 4, 1);
            }
            if (date.Month >= 7 && date.Month <= 9)
            {
                return new DateTime(date.Year, 7, 1);
            }
            else
            {
                return new DateTime(date.Year, 10, 1);
            }
        }

        public static DateTime EndOfQuarter(this DateTime date)
        {
            if (date.Month >= 1 && date.Month <= 3)
            {
                return new DateTime(date.Year, 3, 31);
            }
            if (date.Month >= 4 && date.Month <= 6)
            {
                return new DateTime(date.Year, 6, 30);
            }
            if (date.Month >= 7 && date.Month <= 9)
            {
                return new DateTime(date.Year, 9, 30);
            }
            else
            {
                return new DateTime(date.Year, 12, 31);
            }
        }

        public static int DayOfQuarter(this DateTime date)
        {
            return (int)(date - date.StartOfQuarter()).TotalDays + 1;
        }

        public static int Quarter(this DateTime date)
        {
            if (date.Month >= 1 && date.Month <= 3)
            {
                return 1;
            }
            if (date.Month >= 4 && date.Month <= 6)
            {
                return 2;
            }
            if (date.Month >= 7 && date.Month <= 9)
            {
                return 3;
            }
            else
            {
                return 4;
            }
        }

        public static int HalfYear(this DateTime date)
        {
            if (date.Month >= 1 && date.Month <= 6)
            {
                return 1;
            }
            else
            {
                return 2;
            }
        }

    }
}