using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting
{
    public class DashboardSharing
    {
        public string UserId { get; set; }
        public int DashboardId { get; set; }
        public string Permission { get; set; }
    }
}