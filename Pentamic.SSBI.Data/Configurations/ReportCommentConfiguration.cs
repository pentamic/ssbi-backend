using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class ReportCommentConfiguration : EntityTypeConfiguration<ReportComment>
    {
        public ReportCommentConfiguration()
        {
            HasRequired(x => x.Report).WithMany(x => x.ReportComments);
        }
        //public int Id { get; set; }
        //public int ReportId { get; set; }
        //public string Title { get; set; }
        //public string Content { get; set; }

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }

        //public ReportConfiguration Report { get; set; }
    }
}