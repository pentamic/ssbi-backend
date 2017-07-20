namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Reporting.Dashboard", "GridConfig", c => c.String());
            AddColumn("Reporting.ReportPage", "GridConfig", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Reporting.ReportPage", "GridConfig");
            DropColumn("Reporting.Dashboard", "GridConfig");
        }
    }
}
