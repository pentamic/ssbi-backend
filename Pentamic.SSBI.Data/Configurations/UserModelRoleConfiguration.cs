using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class UserModelRoleConfiguration : EntityTypeConfiguration<UserModelRole>
    {
        public UserModelRoleConfiguration()
        {
            HasKey(x => new { x.UserId, x.RoleId });
            HasRequired(x => x.Role);
        }
        //public string UserId { get; set; }
        //public int RoleId { get; set; }

        //public ModelRoleConfiguration Role { get; set; }
    }
}