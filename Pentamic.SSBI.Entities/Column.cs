using System;
using System.Collections.Generic;

namespace Pentamic.SSBI.Entities
{
    public class Column : IDataModelObject, IAuditable
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SourceColumn { get; set; }
        public string DisplayFolder { get; set; }
        public string FormatString { get; set; }
        public int? SortByColumnId { get; set; }
        public bool IsHidden { get; set; }
        public string Expression { get; set; }
        public ColumnDataType DataType { get; set; }
        public ColumnType ColumnType { get; set; }
        public bool IsKey { get; set; }
        public Table Table { get; set; }

        public List<Relationship> RelationshipFrom { get; set; }
        public List<Relationship> RelationshipTo { get; set; }
        public Column SortByColumn { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}
