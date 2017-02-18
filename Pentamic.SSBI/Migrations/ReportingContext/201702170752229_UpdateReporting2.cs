namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Reporting.ReportTile", "DisplayConfig", c => c.String());
            AddColumn("Reporting.ReportTile", "DataConfig", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Reporting.ReportTile", "DataConfig");
            DropColumn("Reporting.ReportTile", "DisplayConfig");
        }
    }
}
