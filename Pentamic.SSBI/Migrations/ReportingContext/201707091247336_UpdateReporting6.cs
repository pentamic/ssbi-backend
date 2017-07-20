namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Reporting.DashboardTile", "PositionConfig", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Reporting.DashboardTile", "PositionConfig");
        }
    }
}
