using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Services
{
    public class RelationshipDiscoverModel
    {
        public DataSource DataSource { get; set; }
        public string FkTableSchema { get; set; }
        public string FkTableName { get; set; }
    }
}
