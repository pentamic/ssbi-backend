using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class DashboardSharingConfiguration : EntityTypeConfiguration<DashboardSharing>
    {
        public DashboardSharingConfiguration()
        {
            HasKey(x => new { x.UserId, x.DashboardId });
            HasRequired(x => x.Dashboard).WithMany(x => x.DashboardSharings);
        }
        //public string UserId { get; set; }
        //public int DashboardId { get; set; }
        //public string Permission { get; set; }
        //public string SharedBy { get; set; }
        //public DateTimeOffset SharedAt { get; set; }

        //public DashboardConfiguration Dashboard { get; set; }
    }
}