namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting10 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Reporting.Dashboard", "CreatedBy", c => c.String());
            AddColumn("Reporting.Dashboard", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Reporting.Dashboard", "ModifiedBy", c => c.String());
            AddColumn("Reporting.Dashboard", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Reporting.DashboardTile", "CreatedBy", c => c.String());
            AddColumn("Reporting.DashboardTile", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Reporting.DashboardTile", "ModifiedBy", c => c.String());
            AddColumn("Reporting.DashboardTile", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("Reporting.DashboardTile", "ModifiedAt");
            DropColumn("Reporting.DashboardTile", "ModifiedBy");
            DropColumn("Reporting.DashboardTile", "CreatedAt");
            DropColumn("Reporting.DashboardTile", "CreatedBy");
            DropColumn("Reporting.Dashboard", "ModifiedAt");
            DropColumn("Reporting.Dashboard", "ModifiedBy");
            DropColumn("Reporting.Dashboard", "CreatedAt");
            DropColumn("Reporting.Dashboard", "CreatedBy");
        }
    }
}
