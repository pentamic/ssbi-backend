using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class PartitionConfiguration : EntityTypeConfiguration<Partition>
    {
        public PartitionConfiguration()
        {
            HasRequired(x => x.Table).WithMany(x => x.Partitions);
            HasOptional(x => x.DataSource);
        }
        //public int Id { get; set; }
        //public int TableId { get; set; }
        //public int? DataSourceId { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string Query { get; set; }
        //public string Expression { get; set; }
        //public PartitionSourceType SourceType { get; set; }

        //public DateTimeOffset ProcessedAt { get; set; }
        //public string ProcessedBy { get; set; }
        //public bool IsProcessing { get; set; }

        //public TableConfiguration Table { get; set; }
        //public DataSourceConfiguration DataSource { get; set; }

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }
    }
}
