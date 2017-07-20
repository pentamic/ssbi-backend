namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Reporting.ReportTile", "ReportId", c => c.Int(nullable: false));
            AddColumn("Reporting.ReportTile", "ModelId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Reporting.ReportTile", "ModelId");
            DropColumn("Reporting.ReportTile", "ReportId");
        }
    }
}
