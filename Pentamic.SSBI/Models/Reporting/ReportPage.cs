using System;
using System.Collections.Generic;

namespace Pentamic.SSBI.Models.Reporting
{
    public class ReportPage : IAuditable
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public string Name { get; set; }
        public int Ordinal { get; set; }
        public string GridConfig { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public Report Report { get; set; }
        public List<ReportTile> ReportTiles { get; set; }
    }
}