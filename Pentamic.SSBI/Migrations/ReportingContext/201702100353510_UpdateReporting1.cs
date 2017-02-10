namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Reporting.ReportTile", "ColumnFields", c => c.String());
            AddColumn("Reporting.ReportTile", "RowFields", c => c.String());
            AddColumn("Reporting.ReportTile", "ValueFields", c => c.String());
            AddColumn("Reporting.ReportTile", "FilterFields", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Reporting.ReportTile", "FilterFields");
            DropColumn("Reporting.ReportTile", "ValueFields");
            DropColumn("Reporting.ReportTile", "RowFields");
            DropColumn("Reporting.ReportTile", "ColumnFields");
        }
    }
}
