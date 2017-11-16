using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class UserReportActivityConfiguration : EntityTypeConfiguration<UserReportActivity>
    {
        public UserReportActivityConfiguration()
        {
            HasRequired(x => x.Report);
        }
        //public int Id { get; set; }
        //public string UserId { get; set; }
        //public int ReportId { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string CreatedBy { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }

        //public ReportConfiguration Report { get; set; }
    }
}