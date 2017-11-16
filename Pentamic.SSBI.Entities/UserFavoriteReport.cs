using System;

namespace Pentamic.SSBI.Entities
{
    public class UserFavoriteReport : IAuditable
    {
        public string UserId { get; set; }
        public int ReportId { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public Report Report { get; set; }
    }
}