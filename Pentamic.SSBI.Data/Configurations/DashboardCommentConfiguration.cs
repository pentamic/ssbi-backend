using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class DashboardCommentConfiguration : EntityTypeConfiguration<DashboardComment>
    {
        public DashboardCommentConfiguration()
        {
            HasRequired(x => x.Dashboard).WithMany(x => x.DashboardComments);
        }
        //public int Id { get; set; }
        //public int DashboardId { get; set; }
        //public string Title { get; set; }
        //public string Content { get; set; }

        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }

        //public DashboardConfiguration Dashboard { get; set; }
    }
}