using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting
{
    public class ReportFilter : IAuditable
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FilterType { get; set; }
        public string DataConfig { get; set; }
        public string DefaultValue { get; set; }

        public Report Report { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}