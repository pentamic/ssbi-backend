namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting16 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Reporting.UserFavoriteDashboard",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        DashboardId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.DashboardId })
                .ForeignKey("Reporting.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .Index(t => t.DashboardId);
            
            CreateTable(
                "Reporting.UserFavoriteReport",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ReportId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.ReportId })
                .ForeignKey("Reporting.Report", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId);
            
            DropTable("Reporting.UserFavoriteItem");
        }
        
        public override void Down()
        {
            CreateTable(
                "Reporting.UserFavoriteItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ItemId = c.Int(nullable: false),
                        ItemType = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("Reporting.UserFavoriteReport", "ReportId", "Reporting.Report");
            DropForeignKey("Reporting.UserFavoriteDashboard", "DashboardId", "Reporting.Dashboard");
            DropIndex("Reporting.UserFavoriteReport", new[] { "ReportId" });
            DropIndex("Reporting.UserFavoriteDashboard", new[] { "DashboardId" });
            DropTable("Reporting.UserFavoriteReport");
            DropTable("Reporting.UserFavoriteDashboard");
        }
    }
}
