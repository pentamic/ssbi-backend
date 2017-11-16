namespace Pentamic.SSBI.Services
{
    public class DataDiscoverModel
    {
        public int DataSourceId { get; set; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string Query { get; set; }
    }
}