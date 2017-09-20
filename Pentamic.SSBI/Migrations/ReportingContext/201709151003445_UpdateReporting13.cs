namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting13 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Reporting.UserReportActivity",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ReportId = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.ReportId })
                .ForeignKey("Reporting.Report", t => t.ReportId, cascadeDelete: true)
                .Index(t => t.ReportId);
            
            CreateTable(
                "Reporting.UserReportFavorite",
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("Reporting.UserReportFavorite", "ReportId", "Reporting.Report");
            DropForeignKey("Reporting.UserReportActivity", "ReportId", "Reporting.Report");
            DropIndex("Reporting.UserReportFavorite", new[] { "ReportId" });
            DropIndex("Reporting.UserReportActivity", new[] { "ReportId" });
            DropTable("Reporting.UserReportFavorite");
            DropTable("Reporting.UserReportActivity");
        }
    }
}
