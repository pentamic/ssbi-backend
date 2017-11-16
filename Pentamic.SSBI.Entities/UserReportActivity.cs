using System;

namespace Pentamic.SSBI.Entities
{
    public class UserReportActivity : IAuditable
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int ReportId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public Report Report { get; set; }
    }
}