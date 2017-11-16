using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class DashboardConfiguration : EntityTypeConfiguration<Dashboard>
    {
        public DashboardConfiguration()
        {
            
        }
        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public int Ordinal { get; set; }
        //public string GridConfig { get; set; }

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }

        //public List<DashboardTileConfiguration> DashboardTiles { get; set; }
        //public List<DashboardCommentConfiguration> DashboardComments { get; set; }
        //public List<DashboardViewConfiguration> DashboardViews { get; set; }
        //public List<DashboardSharingConfiguration> DashboardSharings { get; set; }
        //public List<DashboardFilterConfiguration> DashboardFilters { get; set; }
    }
}