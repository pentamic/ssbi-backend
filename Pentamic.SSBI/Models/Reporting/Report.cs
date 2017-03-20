﻿using System.Collections.Generic;

namespace Pentamic.SSBI.Models.Reporting
{
    public class Report
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Ordinal { get; set; }

        public List<ReportPage> ReportPages { get; set; }
    }
}