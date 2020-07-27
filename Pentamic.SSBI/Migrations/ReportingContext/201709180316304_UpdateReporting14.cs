namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting14 : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("Reporting.UserReportActivity");
            AddColumn("Reporting.UserReportActivity", "Id", c => c.Int(nullable: false, identity: true));
            AlterColumn("Reporting.UserReportActivity", "UserId", c => c.String());
            AddPrimaryKey("Reporting.UserReportActivity", "Id");
        }
        
        public override void Down()
        {
            DropPrimaryKey("Reporting.UserReportActivity");
            AlterColumn("Reporting.UserReportActivity", "UserId", c => c.String(nullable: false, maxLength: 128));
            DropColumn("Reporting.UserReportActivity", "Id");
            AddPrimaryKey("Reporting.UserReportActivity", new[] { "UserId", "ReportId" });
        }
    }
}
