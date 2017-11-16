using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class ReportConfiguration : EntityTypeConfiguration<Report>
    {
        public ReportConfiguration()
        {
        }
        //public int Id { get; set; }
        //public int ModelId { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public int Ordinal { get; set; }

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }

        //public List<ReportPageConfiguration> ReportPages { get; set; }
        //public List<ReportSharingConfiguration> ReportSharings { get; set; }
        //public List<ReportCommentConfiguration> ReportComments { get; set; }
        //public List<ReportViewConfiguration> ReportViews { get; set; }
        //public List<ReportFilterConfiguration> ReportFilters { get; set; }
    }
}