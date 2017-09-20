using Pentamic.SSBI.Models.DataModel.Objects;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Pentamic.SSBI.Models.DataModel
{
    public class DataModelContext : DbContext
    {
        public DataModelContext() : base("DefaultConnection")
        {
            System.Data.Entity.Database.SetInitializer<DataModelContext>(null);
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
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleTablePermission> RoleTablePermissions { get; set; }
        public DbSet<SourceFile> SourceFiles { get; set; }
        public DbSet<ModelSharing> ModelSharings { get; set; }
        public DbSet<UserModelActivity> UserModelActivities { get; set; }
        public DbSet<UserFavoriteModel> UserFavoriteModels { get; set; }
        public DbSet<ModelProcessQueue> ModelProcessQueues { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("DataModel");
        }
    }
}
