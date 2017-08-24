using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting
{
    public class Dashboard : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GridConfig { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public List<DashboardTile> DashboardTiles { get; set; }
    }
}