using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class UserGroupConfiguration : EntityTypeConfiguration<UserGroup>
    {
        public UserGroupConfiguration()
        {
            HasKey(x => new { x.UserId, x.GroupId });
        }
        //public string UserId { get; set; }
        //public int GroupId { get; set; }
    }
}