using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class ModelRoleTablePermissionConfiguration : EntityTypeConfiguration<ModelRoleTablePermission>
    {
        public ModelRoleTablePermissionConfiguration()
        {
            HasKey(x => new { x.RoleId, x.TableId });
            HasRequired(x => x.Role);
            HasRequired(x => x.Table).WithMany().WillCascadeOnDelete(false);
        }
        //public int RoleId { get; set; }
        //public int TableId { get; set; }
        //public string FilterExpression { get; set; }
        //public bool MetadataPermission { get; set; }

        //public ModelRoleConfiguration Role { get; set; }
        //public TableConfiguration Table { get; set; }
    }
}