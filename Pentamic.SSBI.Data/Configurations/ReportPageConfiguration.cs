using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class ReportPageConfiguration : EntityTypeConfiguration<ReportPage>
    {
        public ReportPageConfiguration()
        {
            HasRequired(x => x.Report).WithMany(x => x.ReportPages);
        }
        //public int Id { get; set; }
        //public int ReportId { get; set; }
        //public string Name { get; set; }
        //public int Ordinal { get; set; }
        //public string GridConfig { get; set; }

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }

        //public ReportConfiguration Report { get; set; }
        //public List<ReportTileConfiguration> ReportTiles { get; set; }
    }
}