using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting
{
    public class Dashboard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string GridConfig { get; set; }

        public List<DashboardTile> DashboardTiles { get; set; }
    }
}