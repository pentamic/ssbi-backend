using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class UserFavoriteDashboardConfiguration : EntityTypeConfiguration<UserFavoriteDashboard>
    {
        public UserFavoriteDashboardConfiguration()
        {
            HasKey(x => new { x.UserId, x.DashboardId });
            HasRequired(x => x.Dashboard);
        }
        //public string UserId { get; set; }
        //public int DashboardId { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }

        //public DashboardConfiguration Dashboard { get; set; }
    }
}