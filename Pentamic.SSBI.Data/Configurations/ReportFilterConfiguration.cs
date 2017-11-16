using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class ReportFilterConfiguration : EntityTypeConfiguration<ReportFilter>
    {
        public ReportFilterConfiguration()
        {
            HasRequired(x => x.Report).WithMany(x => x.ReportFilters);
        }
        //public int Id { get; set; }
        //public int ReportId { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string FilterType { get; set; }
        //public string DataConfig { get; set; }
        //public string DefaultValue { get; set; }

        //public ReportConfiguration Report { get; set; }

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }
    }
}