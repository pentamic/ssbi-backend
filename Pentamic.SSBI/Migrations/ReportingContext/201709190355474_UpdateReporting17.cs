namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting17 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Reporting.UserDashboardActivity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        DashboardId = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Reporting.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .Index(t => t.DashboardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Reporting.UserDashboardActivity", "DashboardId", "Reporting.Dashboard");
            DropIndex("Reporting.UserDashboardActivity", new[] { "DashboardId" });
            DropTable("Reporting.UserDashboardActivity");
        }
    }
}
