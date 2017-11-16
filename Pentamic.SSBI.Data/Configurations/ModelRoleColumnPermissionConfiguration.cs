using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class ModelRoleColumnPermissionConfiguration : EntityTypeConfiguration<ModelRoleColumnPermission>
    {
        public ModelRoleColumnPermissionConfiguration()
        {
            HasKey(x => new { x.RoleId, x.ColumnId });
            HasRequired(x => x.Role);
            HasRequired(x => x.Column).WithMany().WillCascadeOnDelete(false);
        }
        //public int RoleId { get; set; }
        //public int ColumnId { get; set; }
        //public bool MetadataPermission { get; set; }

        //public ModelRoleConfiguration Role { get; set; }
        //public TableConfiguration Column { get; set; }
    }
}