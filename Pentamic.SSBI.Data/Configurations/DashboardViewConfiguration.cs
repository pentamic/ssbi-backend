using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class DashboardViewConfiguration : EntityTypeConfiguration<DashboardView>
    {
        public DashboardViewConfiguration()
        {
            HasRequired(x => x.Dashboard).WithMany(x => x.DashboardViews);
        }
        //public int Id { get; set; }
        //public int DashboardId { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string Selections { get; set; }

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }

        //public DashboardConfiguration Dashboard { get; set; }
    }
}