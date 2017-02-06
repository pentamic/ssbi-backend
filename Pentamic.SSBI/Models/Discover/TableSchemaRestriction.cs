namespace Pentamic.SSBI.Models.Discover
{
    public class TableSchemaRestriction
    {
        public int DataSourceId { get; set; }
        public string TableCatalog { get; set; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }
        public string TableType { get; set; }
        public object[] Restrictions
        {
            get
            {
                return new object[] { TableCatalog, TableSchema, TableName, TableType };
            }
        }
    }
}
