using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(string connString) : base(connString)
        {
        }
        public AppDbContext() : base("Server=.;Database=SSBI_APP_DEV2;Trusted_Connection=True;MultipleActiveResultSets=true")
        {
        }
        public DbSet<Model> Models { get; set; }
        public DbSet<DataSource> DataSources { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Column> Columns { get; set; }
        public DbSet<Relationship> Relationships { get; set; }
        public DbSet<Measure> Measures { get; set; }
        public DbSet<Partition> Partitions { get; set; }
        public DbSet<Perspective> Perspectives { get; set; }
        public DbSet<PerspectiveColumn> PerspectiveColumns { get; set; }
        public DbSet<Hierarchy> Hierarchies { get; set; }
        public DbSet<Level> Levels { get; set; }
        public DbSet<SourceFile> SourceFiles { get; set; }
        public DbSet<ModelSharing> ModelSharings { get; set; }
        public DbSet<UserModelActivity> UserModelActivities { get; set; }
        public DbSet<UserFavoriteModel> UserFavoriteModels { get; set; }
        public DbSet<ModelRefreshQueue> ModelRefreshQueues { get; set; }
        public DbSet<ModelRole> Roles { get; set; }
        public DbSet<ModelRoleTablePermission> RoleTablePermissions { get; set; }
        public DbSet<UserModelRole> UserRoles { get; set; }
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
            modelBuilder.Configurations.AddFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
