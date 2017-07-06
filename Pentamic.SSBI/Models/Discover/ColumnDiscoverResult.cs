using Pentamic.SSBI.Models.DataModel;

namespace Pentamic.SSBI.Models.Discover
{
    public class ColumnDiscoverResult
    {
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
        public string DataType { get; set; }
    }
}
