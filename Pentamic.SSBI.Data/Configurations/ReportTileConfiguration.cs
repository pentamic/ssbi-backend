using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class ReportTileConfiguration : EntityTypeConfiguration<ReportTile>
    {
        public ReportTileConfiguration()
        {
            HasRequired(x => x.ReportPage).WithMany(x => x.ReportTiles);
            HasRequired(x => x.DisplayType);
        }
        //public int Id { get; set; }
        //public int ReportPageId { get; set; }
        //public int ReportId { get; set; }
        //public int ModelId { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string PositionConfig { get; set; }
        //public string DisplayConfig { get; set; }
        //public string DataConfig { get; set; }
        //public string DisplayTypeId { get; set; }
        //public int SourceType { get; set; } //0-Drag&drop.1-BuildRow

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }

        //public ReportPageConfiguration ReportPage { get; set; }
        //public DisplayTypeConfiguration DisplayType { get; set; }
    }
}