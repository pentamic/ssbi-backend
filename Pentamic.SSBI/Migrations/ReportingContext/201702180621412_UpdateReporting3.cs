namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("Reporting.ReportTile", "ColumnFields");
            DropColumn("Reporting.ReportTile", "RowFields");
            DropColumn("Reporting.ReportTile", "ValueFields");
            DropColumn("Reporting.ReportTile", "FilterFields");
        }
        
        public override void Down()
        {
            AddColumn("Reporting.ReportTile", "FilterFields", c => c.String());
            AddColumn("Reporting.ReportTile", "ValueFields", c => c.String());
            AddColumn("Reporting.ReportTile", "RowFields", c => c.String());
            AddColumn("Reporting.ReportTile", "ColumnFields", c => c.String());
        }
    }
}
