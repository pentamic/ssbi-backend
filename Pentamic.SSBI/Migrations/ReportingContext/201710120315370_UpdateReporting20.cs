namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting20 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Reporting.ReportTile", "SourceType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Reporting.ReportTile", "SourceType");
        }
    }
}
