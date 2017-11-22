using System;
using System.Collections.Generic;
using Pentamic.SSBI.Services.Common;

namespace Pentamic.SSBI.Services.SSAS.Dax
{
    public class DaxExpressionGenerator
    {
        public string CreateDateTableExpression(DateTime fromDate, DateTime toDate)
        {
            var dataExpr = BuildDateTableDataExpression(fromDate, toDate);
            var colExpr = BuildDateTableColumnExpression();
            return $"DATATABLE ( {colExpr}, {dataExpr} )";
        }

        private string BuildDateTableColumnExpression()
        {
            var tmp = new[]
{
                "\"DateKey\"", "INTEGER",
                "\"Date\"", "DATETIME",
                "\"DateName\"", "STRING",
                "\"PreviousMonthDate\"", "DATETIME",
                "\"PreviousQuarterDate\"", "DATETIME",
                "\"PreviousYearDate\"", "DATETIME",
                "\"SequentialDayNumber\"", "INTEGER",
                "\"Year\"", "INTEGER",
                "\"YearName\"", "STRING",
                "\"Month\"", "INTEGER",
                "\"MonthName\"", "STRING",
                "\"Quarter\"", "INTEGER",
                "\"QuarterName\"", "STRING",
                "\"HalfYear\"", "INTEGER",
                "\"HalfYearName\"", "STRING",
                "\"DayOfMonth\"", "INTEGER",
                "\"DayOfMonthName\"", "STRING",
                "\"DayOfWeek\"", "INTEGER",
                "\"DayOfWeekName\"", "STRING",
                "\"DayOfYear\"", "INTEGER",
                "\"DayOfQuarter\"", "INTEGER",
                "\"MonthOfYear\"", "INTEGER",
                "\"MonthOfYearName\"", "STRING",
                "\"MonthTotalDays\"", "INTEGER",
                "\"QuarterOfYear\"", "INTEGER",
                "\"QuarterOfYearName\"", "STRING",
                "\"QuarterTotalDays\"", "INTEGER",
                "\"HalfYearOfYear\"", "INTEGER",
                "\"HalfYearOfYearName\"", "STRING",
                "\"LunarDate\"", "INTEGER",
                "\"LunarDateName\"", "STRING",
                "\"LunarMonth\"", "INTEGER",
                "\"LunarMonthName\"", "STRING",
                "\"LunarQuarter\"", "INTEGER",
                "\"LunarQuarterName\"", "STRING",
                "\"LunarYear\"", "INTEGER",
                "\"LunarYearName\"", "STRING",
                "\"LunarDayOfWeek\"", "INTEGER",
                "\"LunarDayOfWeekName\"", "STRING",
                "\"LunarDayOfMonth\"", "INTEGER",
                "\"LunarDayOfMonthName\"", "STRING",
                "\"LunarMonthOfYear\"", "INTEGER",
                "\"LunarMonthOfYearName\"", "STRING",
                "\"LunarQuarterOfYear\"", "INTEGER",
                "\"LunarQuarterOfYearName\"", "STRING",
                "\"EventName\"", "STRING"
            };
            return string.Join(", ", tmp);
        }

        private string BuildDateTableDataExpression(DateTime fromDate, DateTime toDate)
        {
            var dataExpr = new List<string>();
            var i = 0;
            var sdn = 0;
            var smn = 0;
            var cm = 0;
            var sqn = 0;
            var cq = 0;
            while (true)
            {
                var currentDate = fromDate.AddDays(i);
                if (currentDate > toDate)
                {
                    break;
                }
                if (!(currentDate.Month == 2 && currentDate.Day == 29))
                {
                    sdn++;
                }
                if (currentDate.Month != cm)
                {
                    smn++;
                    cm = currentDate.Month;
                }
                if (currentDate.Quarter() != cq)
                {
                    sqn++;
                    cq = currentDate.Quarter();
                }
                var date = new DateData(currentDate)
                {
                    SequentialDayNumber = sdn
                };
                var tmp = new[]
                {
                    date.DateKey.ToString(),
                    "\"" + date.Date.ToString("yyyy-MM-dd") + "\"",
                    "\"" + date.DateName + "\"",
                    "\"" + date.PreviousMonthDate.ToString("yyyy-MM-dd") + "\"",
                    "\"" + date.PreviousQuarterDate.ToString("yyyy-MM-dd") + "\"",
                    "\"" + date.PreviousYearDate.ToString("yyyy-MM-dd") + "\"",
                    date.SequentialDayNumber.ToString(),
                    date.Year.ToString(),
                    "\"" + date.YearName + "\"",
                    date.Month.ToString(),
                    "\"" + date.MonthName + "\"",
                    date.Quarter.ToString(),
                    "\"" + date.QuarterName + "\"",
                    date.HalfYear.ToString(),
                    "\"" + date.HalfYearName + "\"",
                    date.DayOfMonth.ToString(),
                    "\"" + date.DayOfMonthName + "\"",
                    date.DayOfWeek.ToString(),
                    "\"" + date.DayOfWeekName + "\"",
                    date.DayOfYear.ToString(),
                    date.DayOfQuarter.ToString(),
                    date.MonthOfYear.ToString(),
                    "\"" + date.MonthOfYearName + "\"",
                    date.MonthTotalDays.ToString(),
                    date.QuarterOfYear.ToString(),
                    "\"" + date.QuarterOfYearName + "\"",
                    date.QuarterTotalDays.ToString(),
                    date.HalfYearOfYear.ToString(),
                    "\"" + date.HalfYearOfYearName + "\"",
                    date.LunarDate.ToString(),
                    "\"" + date.LunarDateName + "\"",
                    date.LunarMonth.ToString(),
                    "\"" + date.LunarMonthName + "\"",
                    date.LunarQuarter.ToString(),
                    "\"" + date.LunarQuarterName + "\"",
                    date.LunarYear.ToString(),
                    "\"" + date.LunarYearName + "\"",
                    date.LunarDayOfWeek.ToString(),
                    "\"" + date.LunarDayOfWeekName + "\"",
                    date.LunarDayOfMonth.ToString(),
                    "\"" + date.LunarDayOfMonthName + "\"",
                    date.LunarMonthOfYear.ToString(),
                    "\"" + date.LunarMonthOfYearName + "\"",
                    date.LunarQuarterOfYear.ToString(),
                    "\"" + date.LunarQuarterOfYearName + "\"",
                    "\"" + date.EventName + "\""
                };
                dataExpr.Add(" { " + string.Join(", ", tmp) + " } ");
                i++;
            }
            return "{" + string.Join(", ", dataExpr) + "}";
        }
    }
}
