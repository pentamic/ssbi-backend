using System.Collections.Generic;

namespace Pentamic.SSBI.Models.DataModel
{
    public class Table : IDataModelObject
    {
        public int Id { get; set; }
        public int DataSourceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OriginalName { get; set; }
        public string SourceTable { get; set; }
        public string SourceSchema { get; set; }
        public DataModelObjectState State { get; set; }
        public List<Column> Columns { get; set; }
        public List<Partition> Partitions { get; set; }
        public List<Measure> Measures { get; set; }
        public List<Hierarchy> Hierarchies { get; set; }
        public DataSource DataSource { get; set; }
    }
}
