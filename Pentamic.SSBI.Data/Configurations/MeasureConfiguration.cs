using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class MeasureConfiguration : EntityTypeConfiguration<Measure>
    {
        public MeasureConfiguration()
        {
            HasRequired(x => x.Table).WithMany(x => x.Measures);
        }
        //public int Id { get; set; }
        //public int TableId { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string DisplayFolder { get; set; }
        //public string Expression { get; set; }
        //public string FormatString { get; set; }
        //public TableConfiguration Table { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }
    }
}
