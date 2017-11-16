using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class UserFavoriteReportConfiguration : EntityTypeConfiguration<UserFavoriteReport>
    {
        public UserFavoriteReportConfiguration()
        {
            HasKey(x => new { x.UserId, x.ReportId });
            HasRequired(x => x.Report);
        }
        //public string UserId { get; set; }
        //public int ReportId { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }

        //public ReportConfiguration Report { get; set; }
    }
}