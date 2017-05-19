namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Reporting.ReportTile", "PositionConfig", c => c.String());
            AddColumn("Reporting.ReportTile", "DisplayType", c => c.Int(nullable: false));
            DropColumn("Reporting.ReportTile", "Column");
            DropColumn("Reporting.ReportTile", "Row");
            DropColumn("Reporting.ReportTile", "SizeX");
            DropColumn("Reporting.ReportTile", "SizeY");
        }
        
        public override void Down()
        {
            AddColumn("Reporting.ReportTile", "SizeY", c => c.Int(nullable: false));
            AddColumn("Reporting.ReportTile", "SizeX", c => c.Int(nullable: false));
            AddColumn("Reporting.ReportTile", "Row", c => c.Int(nullable: false));
            AddColumn("Reporting.ReportTile", "Column", c => c.Int(nullable: false));
            DropColumn("Reporting.ReportTile", "DisplayType");
            DropColumn("Reporting.ReportTile", "PositionConfig");
        }
    }
}
