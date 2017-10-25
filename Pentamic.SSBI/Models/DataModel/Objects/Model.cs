using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class Model : IDataModelObject, IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DatabaseName { get; set; }
        public ModeType DefaultMode { get; set; }
        public string GenerateFromTemplate { get; set; }
        public int? CloneFromModelId { get; set; }

        public string RefreshSchedule { get; set; }
        public string RefreshJobId { get; set; }

        public List<Table> Tables { get; set; }
        public List<DataSource> DataSources { get; set; }
        public List<Relationship> Relationships { get; set; }
        public List<Role> Roles { get; set; }
        public List<ModelSharing> ModelSharings { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public DateTimeOffset RefreshedAt { get; set; }

    }
}
