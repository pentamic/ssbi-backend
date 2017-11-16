using System;

namespace Pentamic.SSBI.Entities
{
    public class ReportComment : IAuditable
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public Report Report { get; set; }
    }
}