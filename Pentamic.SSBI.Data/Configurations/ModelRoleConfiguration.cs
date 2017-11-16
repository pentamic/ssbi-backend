using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class ModelRoleConfiguration : EntityTypeConfiguration<ModelRole>
    {
        public ModelRoleConfiguration()
        {
            HasRequired(x => x.Model).WithMany(x => x.Roles);
        }
        //public int Id { get; set; }
        //public int ModelId { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public ModelPermission ModelPermission { get; set; }

        //public ModelConfiguration Model { get; set; }
        //public List<ModelRoleTablePermissionConfiguration> TablePermissions { get; set; }
        //public List<UserModelRoleConfiguration> Users { get; set; }

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }
    }
}