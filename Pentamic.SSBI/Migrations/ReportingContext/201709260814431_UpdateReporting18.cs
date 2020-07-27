namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting18 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Reporting.DashboardFilter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DashboardId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        FilterType = c.String(),
                        DataConfig = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Reporting.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .Index(t => t.DashboardId);
            
            CreateTable(
                "Reporting.ReportFilter",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        FilterType = c.String(),
                        DataConfig = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Reporting.Report", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Reporting.ReportFilter", "ReportId", "Reporting.Report");
            DropForeignKey("Reporting.DashboardFilter", "DashboardId", "Reporting.Dashboard");
            DropIndex("Reporting.ReportFilter", new[] { "ReportId" });
            DropIndex("Reporting.DashboardFilter", new[] { "DashboardId" });
            DropTable("Reporting.ReportFilter");
            DropTable("Reporting.DashboardFilter");
        }
    }
}
