using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting
{
    public class ReportPage
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public string Name { get; set; }
        public int Ordinal { get; set; }
    }
}