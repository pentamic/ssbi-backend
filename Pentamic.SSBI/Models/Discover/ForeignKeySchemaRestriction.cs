namespace Pentamic.SSBI.Models.Discover
{
    public class ForeignKeySchemaRestriction
    {
        public int DataSourceId { get; set; }
        public string PkTableCatalog { get; set; }
        public string PkTableSchema { get; set; }
        public string PkTableName { get; set; }
        public string FkTableCatalog { get; set; }
        public string FkTableSchema { get; set; }
        public string FkTableName { get; set; }
        public object[] Restrictions
        {
            get
            {
                return new object[] { PkTableCatalog, PkTableSchema, PkTableName, FkTableCatalog, FkTableSchema, FkTableName };
            }
        }
    }
}
