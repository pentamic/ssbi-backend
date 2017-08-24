namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Reporting.DashboardSharing",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        DashboardId = c.Int(nullable: false),
                        Permission = c.String(),
                        SharedBy = c.String(),
                        SharedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.DashboardId })
                .ForeignKey("Reporting.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .Index(t => t.DashboardId);
            
            CreateTable(
                "Reporting.ReportSharing",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ReportId = c.Int(nullable: false),
                        Permission = c.String(),
                        SharedBy = c.String(),
                        SharedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.ReportId })
                .ForeignKey("Reporting.Report", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Reporting.ReportSharing", "ReportId", "Reporting.Report");
            DropForeignKey("Reporting.DashboardSharing", "DashboardId", "Reporting.Dashboard");
            DropIndex("Reporting.ReportSharing", new[] { "ReportId" });
            DropIndex("Reporting.DashboardSharing", new[] { "DashboardId" });
            DropTable("Reporting.ReportSharing");
            DropTable("Reporting.DashboardSharing");
        }
    }
}
