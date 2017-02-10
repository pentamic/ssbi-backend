using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Pentamic.SSBI.Models.Reporting
{
    public class ReportingContext : DbContext
    {
        public ReportingContext() : base("DefaultConnection")
        {
            System.Data.Entity.Database.SetInitializer<ReportingContext>(null);
        }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportPage> ReportPages { get; set; }
        public DbSet<ReportTile> ReportTiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("Reporting");
        }
    }
}
