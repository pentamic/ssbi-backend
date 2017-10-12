using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting
{
    public class ReportTileRow
    {
        public int Id { get; set; }
        public int ReportTileId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Ordinal { get; set; }
        public bool IsGroup { get; set; }
        public int? ParentId { get; set; }
        public bool IsFormula { get; set; }
        public string ValueExpression { get; set; }
        public string FilterExpression { get; set; }
        public string FormulaExpression { get; set; }
        public int Indent { get; set; }
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderline { get; set; }
        public bool IsUppercase { get; set; }
        public string BackgroundColor { get; set; }
        public string TextColor { get; set; }

        public ReportTile ReportTile { get; set; }
    }
}