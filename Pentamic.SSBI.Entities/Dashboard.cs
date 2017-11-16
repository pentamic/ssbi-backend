using System;
using System.Collections.Generic;

namespace Pentamic.SSBI.Entities
{
    public class Dashboard : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Ordinal { get; set; }
        public string GridConfig { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public List<DashboardTile> DashboardTiles { get; set; }
        public List<DashboardComment> DashboardComments { get; set; }
        public List<DashboardView> DashboardViews { get; set; }
        public List<DashboardSharing> DashboardSharings { get; set; }
        public List<DashboardFilter> DashboardFilters { get; set; }
    }
}