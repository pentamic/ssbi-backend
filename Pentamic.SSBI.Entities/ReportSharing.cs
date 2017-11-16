using System;

namespace Pentamic.SSBI.Entities
{
    public class ReportSharing : IShareInfo
    {
        public string UserId { get; set; }
        public int ReportId { get; set; }
        public string Permission { get; set; }
        public string SharedBy { get; set; }
        public DateTimeOffset SharedAt { get; set; }

        public Report Report { get; set; }
    }
}