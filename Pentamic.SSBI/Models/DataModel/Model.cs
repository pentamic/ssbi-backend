using System.Collections.Generic;

namespace Pentamic.SSBI.Models.DataModel
{
    public class Model : IDataModelObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OriginalName { get; set; }
        public string DatabaseName { get; set; }

        public DataModelObjectState State { get; set; }
        public List<DataSource> DataSources { get; set; }
        public List<Relationship> Relationships { get; set; }
    }
}
