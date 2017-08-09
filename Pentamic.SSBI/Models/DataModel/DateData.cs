using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel
{
    public class DateData
    {
        public int DateKey { get; set; }
        public DateTime Date { get; set; }
        public string DateName { get; set; }
        public int Year { get; set; }
        public string YearName { get; set; }
        public int Month { get; set; }
        public string MonthName { get; set; }
        public int Quarter { get; set; }
        public string QuarterName { get; set; }
        public int HalfYear { get; set; }
        public string HalfYearName { get; set; }
        public int DayOfMonth { get; set; }
        public string DayOfMonthName { get; set; }
        public int DayOfWeek { get; set; }
        public string DayOfWeekName { get; set; }
        public int MonthOfYear { get; set; }
        public string MonthOfYearName { get; set; }
        public int QuarterOfYear { get; set; }
        public string QuarterOfYearName { get; set; }
        public int HalfYearOfYear { get; set; }
        public string HalfYearOfYearName { get; set; }
        public int LunarDate { get; set; }
        public string LunarDateName { get; set; }
        public int LunarMonth { get; set; }
        public string LunarMonthName { get; set; }
        public int LunarQuarter { get; set; }
        public string LunarQuarterName { get; set; }
        public int LunarYear { get; set; }
        public string LunarYearName { get; set; }
        public int LunarDayOfWeek { get; set; }
        public string LunarDayOfWeekName { get; set; }
        public int LunarDayOfMonth { get; set; }
        public string LunarDayOfMonthName { get; set; }
        public int LunarMonthOfYear { get; set; }
        public string LunarMonthOfYearName { get; set; }
        public int LunarQuarterOfYear { get; set; }
        public string LunarQuarterOfYearName { get; set; }
        public string EventName { get; set; }

        public DateData(DateTime date)
        {
            Date = date;
            DateKey = date.Year * 10000 + date.Month * 100 + date.Day;
            DateName = date.ToString("dd/MM/yyyy");
            DayOfMonth = date.Day;
            DayOfMonthName = "Ngày " + DayOfMonth;
            Year = date.Year;
            YearName = "Năm " + date.Year;
            MonthOfYear = date.Month;
            MonthOfYearName = "Tháng " + MonthOfYear;
            Month = date.Year * 100 + MonthOfYear;
            MonthName = "Tháng " + MonthOfYear + " năm " + Year;
            if (MonthOfYear >= 1 && MonthOfYear <= 3)
            {
                QuarterOfYear = 1;
                HalfYearOfYear = 1;
            }
            if (MonthOfYear >= 4 && MonthOfYear <= 6)
            {
                QuarterOfYear = 2;
                HalfYearOfYear = 1;
            }
            if (MonthOfYear >= 7 && MonthOfYear <= 9)
            {
                QuarterOfYear = 3;
                HalfYearOfYear = 2;
            }
            if (MonthOfYear >= 10 && MonthOfYear <= 12)
            {
                QuarterOfYear = 4;
                HalfYearOfYear = 2;
            }
            QuarterOfYearName = "Quý " + QuarterOfYear;
            HalfYearOfYearName = "Kỳ " + HalfYearOfYear;
            Quarter = Year * 100 + QuarterOfYear;
            HalfYear = Year * 100 + HalfYearOfYear;
            QuarterName = "Quý " + QuarterOfYear + " năm " + Year;
            HalfYearName = "Kỳ " + HalfYearOfYear + " năm " + Year;
            DayOfWeek = (int)date.DayOfWeek;
            switch (DayOfWeek)
            {
                case 0: DayOfWeekName = "Chủ nhật"; break;
                case 1: DayOfWeekName = "Thứ 2"; break;
                case 2: DayOfWeekName = "Thứ 3"; break;
                case 3: DayOfWeekName = "Thứ 4"; break;
                case 4: DayOfWeekName = "Thứ 5"; break;
                case 5: DayOfWeekName = "Thứ 6"; break;
                case 6: DayOfWeekName = "Thứ 7"; break;
                default: break;
            }
            var lunarCalendar = new ChineseLunisolarCalendar();
            LunarYear = lunarCalendar.GetYear(date);
            LunarYearName = "Năm " + LunarYear;
            LunarMonthOfYear = lunarCalendar.GetMonth(date);
            LunarMonth = LunarYear * 100 + LunarMonthOfYear;
            LunarDayOfMonth = lunarCalendar.GetDayOfMonth(date);
            LunarDayOfMonthName = "Ngày " + LunarDayOfMonth;
            LunarDate = LunarYear * 10000 + LunarMonthOfYear * 100 + LunarDayOfMonth;
            LunarDateName = $"{LunarDayOfMonth}/{LunarMonthOfYear}/{LunarYear}";
            LunarMonthName = "Tháng " + LunarMonthOfYear + " năm " + LunarYear;
            LunarMonthOfYearName = "Tháng " + LunarMonthOfYear;

            if (LunarMonthOfYear >= 1 && LunarMonthOfYear <= 3)
            {
                LunarQuarterOfYear = 1;
            }
            if (LunarMonthOfYear >= 4 && LunarMonthOfYear <= 6)
            {
                LunarQuarterOfYear = 2;
            }
            if (LunarMonthOfYear >= 7 && LunarMonthOfYear <= 9)
            {
                LunarQuarterOfYear = 3;
            }
            if (LunarMonthOfYear >= 10)
            {
                LunarQuarterOfYear = 4;
            }
            LunarQuarterOfYearName = "Quý " + LunarQuarterOfYear;
            LunarQuarter = LunarYear * 100 + LunarQuarterOfYear;
            LunarQuarterName = "Quý " + LunarQuarterOfYear + " năm " + LunarYear;
            LunarDayOfWeek = (int)lunarCalendar.GetDayOfWeek(date);
            switch (LunarDayOfWeek)
            {
                case 0: LunarDayOfWeekName = "Chủ nhật"; break;
                case 1: LunarDayOfWeekName = "Thứ 2"; break;
                case 2: LunarDayOfWeekName = "Thứ 3"; break;
                case 3: LunarDayOfWeekName = "Thứ 4"; break;
                case 4: LunarDayOfWeekName = "Thứ 5"; break;
                case 5: LunarDayOfWeekName = "Thứ 6"; break;
                case 6: LunarDayOfWeekName = "Thứ 7"; break;
                default: break;
            }
        }
    }
}