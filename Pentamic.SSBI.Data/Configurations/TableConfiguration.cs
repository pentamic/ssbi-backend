using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class TableConfiguration : EntityTypeConfiguration<Table>
    {
        public TableConfiguration()
        {
            HasRequired(x => x.Model).WithMany(x => x.Tables);
        }
        //public int Id { get; set; }
        //public int ModelId { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string SourceTable { get; set; }
        //public string SourceSchema { get; set; }
        //public string DataCategory { get; set; }

        //public DateTimeOffset ProcessedAt { get; set; }
        //public string ProcessedBy { get; set; }
        //public bool IsProcessing { get; set; }

        //public List<ColumnConfiguration> Columns { get; set; }
        //public List<PartitionConfiguration> Partitions { get; set; }
        //public List<MeasureConfiguration> Measures { get; set; }
        //public List<HierarchyConfiguration> Hierarchies { get; set; }
        //public ModelConfiguration Model { get; set; }


        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }
    }
}
