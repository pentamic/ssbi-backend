using System;
using System.Collections.Generic;
using Pentamic.SSBI.Models.DataModel.Connections;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class DataSource : IDataModelObject, IAuditable
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public int ConnectionId { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public string Description { get; set; }
        public string ConnectionString { get; set; }

        public List<Partition> Partitions { get; set; }
        public Model Model { get; set; }
        public Connection Connection { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}
