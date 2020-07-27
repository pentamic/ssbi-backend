namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Reporting.DashboardComment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DashboardId = c.Int(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Reporting.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .Index(t => t.DashboardId);
            
            CreateTable(
                "Reporting.ReportComment",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportId = c.Int(nullable: false),
                        Title = c.String(),
                        Content = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Reporting.Report", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId);
            
            CreateTable(
                "Reporting.ReportView",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Selections = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Reporting.Report", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId);
            
            CreateTable(
                "Reporting.DashboardView",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        DashboardId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Selections = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Reporting.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .Index(t => t.DashboardId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Reporting.DashboardView", "DashboardId", "Reporting.Dashboard");
            DropForeignKey("Reporting.ReportView", "ReportId", "Reporting.Report");
            DropForeignKey("Reporting.ReportComment", "ReportId", "Reporting.Report");
            DropForeignKey("Reporting.DashboardComment", "DashboardId", "Reporting.Dashboard");
            DropIndex("Reporting.DashboardView", new[] { "DashboardId" });
            DropIndex("Reporting.ReportView", new[] { "ReportId" });
            DropIndex("Reporting.ReportComment", new[] { "ReportId" });
            DropIndex("Reporting.DashboardComment", new[] { "DashboardId" });
            DropTable("Reporting.DashboardView");
            DropTable("Reporting.ReportView");
            DropTable("Reporting.ReportComment");
            DropTable("Reporting.DashboardComment");
        }
    }
}
