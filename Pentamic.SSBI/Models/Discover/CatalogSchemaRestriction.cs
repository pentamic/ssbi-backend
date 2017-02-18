namespace Pentamic.SSBI.Models.Discover
{
    public class CatalogSchemaRestriction
    {
        public int DataSourceId { get; set; }
        public string CatalogName { get; set; }
        public object[] Restrictions
        {
            get
            {
                return new object[] { CatalogName };
            }
        }
    }
}
