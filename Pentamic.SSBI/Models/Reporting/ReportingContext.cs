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

        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<DashboardTile> DashboardTiles { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportPage> ReportPages { get; set; }
        public DbSet<ReportTile> ReportTiles { get; set; }
        public DbSet<DisplayType> DisplayTypes { get; set; }

        public DbSet<ReportSharing> ReportSharings { get; set; }
        public DbSet<DashboardSharing> DashboardSharings { get; set; }

        public DbSet<DashboardComment> DashboardComments { get; set; }
        public DbSet<ReportComment> ReportComments { get; set; }

        public DbSet<DashboardView> DashboardViews { get; set; }
        public DbSet<ReportView> ReportViews { get; set; }

        public DbSet<ReportFilter> ReportFilters { get; set; }
        public DbSet<DashboardFilter> DashboardFilters { get; set; }

        public DbSet<UserReportActivity> UserReportActivities { get; set; }
        public DbSet<UserDashboardActivity> UserDashboardActivities { get; set; }

        public DbSet<UserFavoriteReport> UserFavoriteReports { get; set; }
        public DbSet<UserFavoriteDashboard> UserFavoriteDashboards { get; set; }

        public DbSet<ReportTileRow> ReportTileRows { get; set; }

        public DbSet<Alert> Alerts { get; set; }

        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationSubscription> NotificationSubscriptions { get; set; }
        public DbSet<UserNotification> UserNotifications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("Reporting");
        }
    }
}
