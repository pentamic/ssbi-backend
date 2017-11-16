using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class DashboardTileConfiguration : EntityTypeConfiguration<DashboardTile>
    {
        public DashboardTileConfiguration()
        {
            HasKey(x => new { x.DashboardId, x.ReportTileId });
            HasRequired(x => x.Dashboard).WithMany(x => x.DashboardTiles);
            HasRequired(x => x.ReportTile).WithMany();
        }
        //public int DashboardId { get; set; }
        //public int ReportTileId { get; set; }
        //public string PositionConfig { get; set; }

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }

        //public DashboardConfiguration Dashboard { get; set; }
        //public ReportTileConfiguration ReportTile { get; set; }

    }
}