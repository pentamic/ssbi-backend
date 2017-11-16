using System;

namespace Pentamic.SSBI.Entities
{
    public class DashboardTile : IAuditable
    {
        public int DashboardId { get; set; }
        public int ReportTileId { get; set; }
        public string PositionConfig { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public Dashboard Dashboard { get; set; }
        public ReportTile ReportTile { get; set; }

    }
}