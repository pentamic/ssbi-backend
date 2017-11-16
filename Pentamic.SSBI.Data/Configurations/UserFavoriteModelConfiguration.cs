using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class UserFavoriteModelConfiguration : EntityTypeConfiguration<UserFavoriteModel>
    {
        public UserFavoriteModelConfiguration()
        {
            HasKey(x => new { x.UserId, x.ModelId });
            HasRequired(x => x.Model);
        }
        //public string UserId { get; set; }
        //public int ModelId { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }

        //public ModelConfiguration Model { get; set; }
    }
}