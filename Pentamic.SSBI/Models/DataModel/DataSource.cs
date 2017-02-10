using System.Collections.Generic;

namespace Pentamic.SSBI.Models.DataModel
{
    public class DataSource : IDataModelObject
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Description { get; set; }
        public DataSourceType Type { get; set; }
        public string Source { get; set; }
        public string Catalog { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool IntegratedSecurity { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DataModelObjectState State { get; set; }
        public Model Model { get; set; }
        public List<Table> Tables { get; set; }
    }
}
