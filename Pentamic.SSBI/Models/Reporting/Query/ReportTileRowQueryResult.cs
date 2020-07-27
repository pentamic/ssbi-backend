using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting.Query
{
    public class ReportTileRowQueryResult
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Ordinal { get; set; }
        public bool IsGroup { get; set; }
        public int? ParentId { get; set; }
        public bool IsFormula { get; set; }
        public string FormulaExpression { get; set; }
        public int Indent { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderline { get; set; }
        public bool IsUppercase { get; set; }
        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }
        public decimal PMTD { get; set; }
        public decimal MTD { get; set; }
        public decimal MTDRate { get; set; }
        public decimal PYTD { get; set; }
        public decimal YTD { get; set; }
        public decimal YTDRate { get; set; }
        public bool IsCalculated { get; set; }

        public ReportTileRowQueryResult() { }

        public ReportTileRowQueryResult(ReportTileRow r)
        {
            Id = r.Id;
            Code = r.Code;
            Name = r.Name;
            Ordinal = r.Ordinal;
            IsGroup = r.IsGroup;
            ParentId = r.ParentId;
            IsFormula = r.IsFormula;
            Indent = r.Indent;
            IsBold = r.IsBold;
            IsItalic = r.IsItalic;
            IsUnderline = r.IsUnderline;
            IsUppercase = r.IsUppercase;
            BackgroundColor = r.BackgroundColor;
            TextColor = r.TextColor;
            FormulaExpression = r.FormulaExpression;
        }

        public static ReportTileRowQueryResult operator +(ReportTileRowQueryResult m1, ReportTileRowQueryResult m2)
        {
            return new ReportTileRowQueryResult
            {
                MTD = m1.MTD + m2.MTD,
                PMTD = m1.PMTD + m2.PMTD,
                YTD = m1.YTD + m2.YTD,
                PYTD = m1.PYTD + m2.PYTD
            };
        }
        public static ReportTileRowQueryResult operator -(ReportTileRowQueryResult m1, ReportTileRowQueryResult m2)
        {
            return new ReportTileRowQueryResult
            {
                MTD = m1.MTD - m2.MTD,
                PMTD = m1.PMTD - m2.PMTD,
                YTD = m1.YTD - m2.YTD,
                PYTD = m1.PYTD - m2.PYTD
            };
        }
    }
}