using System;
using System.Collections.Generic;

namespace Pentamic.SSBI.Entities
{
    public class Report : IAuditable
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Ordinal { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public List<ReportPage> ReportPages { get; set; }
        public List<ReportSharing> ReportSharings { get; set; }
        public List<ReportComment> ReportComments { get; set; }
        public List<ReportView> ReportViews { get; set; }
        public List<ReportFilter> ReportFilters { get; set; }
    }
}