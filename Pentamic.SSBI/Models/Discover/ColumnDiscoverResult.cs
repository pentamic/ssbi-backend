using Pentamic.SSBI.Models.DataModel;

namespace Pentamic.SSBI.Models.Discover
{
    public class ColumnDiscoverResult
    {
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string Name { get; set; }
        public ColumnDataType DataType { get; set; }
    }
}
