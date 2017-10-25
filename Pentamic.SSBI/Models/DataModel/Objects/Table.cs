using System;
using System.Collections.Generic;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class Table : IDataModelObject, IAuditable
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string SourceTable { get; set; }
        public string SourceSchema { get; set; }
        public string DataCategory { get; set; }

        public DateTimeOffset ProcessedAt { get; set; }
        public string ProcessedBy { get; set; }
        public bool IsProcessing { get; set; }

        public List<Column> Columns { get; set; }
        public List<Partition> Partitions { get; set; }
        public List<Measure> Measures { get; set; }
        public List<Hierarchy> Hierarchies { get; set; }
        public Model Model { get; set; }


        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}
