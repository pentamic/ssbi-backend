using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class PerspectiveColumnConfiguration : EntityTypeConfiguration<PerspectiveColumn>
    {
        public PerspectiveColumnConfiguration()
        {
            HasKey(x => new { x.PerspectiveId, x.ColumnId });
        }
        //public int PerspectiveId { get; set; }
        //public int ColumnId { get; set; }

        //public PerspectiveConfiguration Perspective { get; set; }
        //public ColumnConfiguration Column { get; set; }
    }
}