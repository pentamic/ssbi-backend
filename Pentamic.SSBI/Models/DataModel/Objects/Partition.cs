using System;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class Partition : DataModelObjectBase
    {
        public int ModelId { get; set; }
        public int TableId { get; set; }
        public int? DataSourceId { get; set; }
        public string Description { get; set; }
        public string Query { get; set; }
        public string Expression { get; set; }
        public PartitionSourceType SourceType { get; set; }

        public DateTimeOffset ProcessedAt { get; set; }
        public string ProcessedBy { get; set; }
        public bool IsProcessing { get; set; }

        public Table Table { get; set; }
        public DataSource DataSource { get; set; }
    }
}
