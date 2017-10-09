using System;
using System.Collections.Generic;
using Pentamic.SSBI.Models.DataModel.Objects;

namespace Pentamic.SSBI.Models.DataModel.Connections
{
    public abstract class Connection : IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public List<ModelConnection> ModelConnections { get; set; }
    }
}