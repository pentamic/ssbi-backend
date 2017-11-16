using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class ColumnConfiguration : EntityTypeConfiguration<Column>
    {
        public ColumnConfiguration()
        {
            HasRequired(x => x.Table).WithMany(x => x.Columns);
        }
        //public int Id { get; set; }
        //public int TableId { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string SourceColumn { get; set; }
        //public string DisplayFolder { get; set; }
        //public string FormatString { get; set; }
        //public int? SortByColumnId { get; set; }
        //public bool IsHidden { get; set; }
        //public string Expression { get; set; }
        //public ColumnDataType DataType { get; set; }
        //public ColumnType ColumnType { get; set; }
        //public bool IsKey { get; set; }
        //public TableConfiguration Table { get; set; }

        //public List<RelationshipConfiguration> RelationshipFrom { get; set; }
        //public List<RelationshipConfiguration> RelationshipTo { get; set; }
        //public ColumnConfiguration SortByColumn { get; set; }

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }
    }
}
