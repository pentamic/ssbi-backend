using System;
using System.Collections.Generic;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class DataSource : DataModelObjectBase
    {
        public int ModelId { get; set; }
        public string Description { get; set; }
        public DataSourceType Type { get; set; }
        public string ConnectionString { get; set; }
        public string Source { get; set; }
        public string Catalog { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public bool IntegratedSecurity { get; set; }
        public int? SourceFileId { get; set; }

        public List<Partition> Partitions { get; set; }
        public Model Model { get; set; }
        public SourceFile SourceFile { get; set; }
    }
}
