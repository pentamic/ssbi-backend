namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting : DbMigration
    {
        public override void Up()
        {
            CreateIndex("Reporting.ReportPage", "ReportId");
            CreateIndex("Reporting.ReportTile", "ReportPageId");
            AddForeignKey("Reporting.ReportPage", "ReportId", "Reporting.Report", "Id", cascadeDelete: true);
            AddForeignKey("Reporting.ReportTile", "ReportPageId", "Reporting.ReportPage", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("Reporting.ReportTile", "ReportPageId", "Reporting.ReportPage");
            DropForeignKey("Reporting.ReportPage", "ReportId", "Reporting.Report");
            DropIndex("Reporting.ReportTile", new[] { "ReportPageId" });
            DropIndex("Reporting.ReportPage", new[] { "ReportId" });
        }
    }
}
