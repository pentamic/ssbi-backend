using Pentamic.SSBI.Models.DataModel.Objects;
using System.Collections.Generic;
using System;

namespace Pentamic.SSBI.Models.Reporting
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
    }
}