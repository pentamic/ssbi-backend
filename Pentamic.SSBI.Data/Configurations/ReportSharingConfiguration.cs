using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class ReportSharingConfiguration : EntityTypeConfiguration<ReportSharing>
    {
        public ReportSharingConfiguration()
        {
            HasKey(x => new { x.UserId, x.ReportId });
            HasRequired(x => x.Report);
        }
        //public string UserId { get; set; }
        //public int ReportId { get; set; }
        //public string Permission { get; set; }
        //public string SharedBy { get; set; }
        //public DateTimeOffset SharedAt { get; set; }

        //public ReportConfiguration Report { get; set; }
    }
}