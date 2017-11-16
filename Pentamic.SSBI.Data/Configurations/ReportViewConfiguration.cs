using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class ReportViewConfiguration : EntityTypeConfiguration<ReportView>
    {
        public ReportViewConfiguration()
        {
            HasRequired(x => x.Report).WithMany(x => x.ReportViews);
        }
        //public int Id { get; set; }
        //public int ReportId { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string Selections { get; set; }

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }

        //public ReportConfiguration Report { get; set; }
    }
}