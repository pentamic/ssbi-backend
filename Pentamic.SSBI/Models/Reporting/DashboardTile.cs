using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting
{
    public class DashboardTile : IAuditable
    {
        [Key]
        [Column(Order = 1)]
        public int DashboardId { get; set; }
        [Key]
        [Column(Order = 2)]
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