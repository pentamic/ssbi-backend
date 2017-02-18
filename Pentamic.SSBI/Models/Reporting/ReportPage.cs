using System.Collections.Generic;

namespace Pentamic.SSBI.Models.Reporting
{
    public class ReportPage
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public string Name { get; set; }
        public int Ordinal { get; set; }

        public Report Report { get; set; }
        public List<ReportTile> ReportTiles { get; set; }
    }
}