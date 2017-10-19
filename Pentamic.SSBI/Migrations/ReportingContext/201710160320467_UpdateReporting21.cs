namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting21 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("Reporting.ReportTileRow", "Ordinal", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("Reporting.ReportTileRow", "Ordinal", c => c.String());
        }
    }
}
