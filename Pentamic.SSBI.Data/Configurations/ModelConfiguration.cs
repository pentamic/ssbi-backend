using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class ModelConfiguration : EntityTypeConfiguration<Model>
    {
        public ModelConfiguration() { }
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public ModeType DefaultMode { get; set; }
        //public string GenerateFromTemplate { get; set; }
        //public int? CloneFromModelId { get; set; }

        //public string RefreshSchedule { get; set; }
        //public string RefreshJobId { get; set; }

        //public List<TableConfiguration> Tables { get; set; }
        //public List<DataSourceConfiguration> DataSources { get; set; }
        //public List<RelationshipConfiguration> Relationships { get; set; }
        //public List<ModelRoleConfiguration> Roles { get; set; }
        //public List<ModelSharingConfiguration> ModelSharings { get; set; }

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }
        //public DateTimeOffset RefreshedAt { get; set; }

    }
}
