using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pentamic.SSBI.Models.DataModel
{
    public class Column : IDataModelObject
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string UpdatedProperties { get; set; }
        public string SourceColumn { get; set; }
        public ColumnDataType DataType { get; set; }
        public DataModelObjectState State { get; set; }
        public string OriginalValuesMap { get; set; }
        public Table Table { get; set; }
        [InverseProperty("FromColumn")]
        public List<Relationship> RelationshipFrom { get; set; }
        [InverseProperty("ToColumn")]
        public List<Relationship> RelationshipTo { get; set; }
    }
}
