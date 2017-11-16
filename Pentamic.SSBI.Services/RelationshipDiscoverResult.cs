namespace Pentamic.SSBI.Services
{
    public class RelationshipDiscoverResult
    {
        public string PkTableSchema { get; set; }
        public string PkTableName { get; set; }
        public string FkTableSchema { get; set; }
        public string FkTableName { get; set; }
    }
}