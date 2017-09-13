namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting12 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Reporting.Dashboard", "Description", c => c.String());
            AddColumn("Reporting.Dashboard", "Ordinal", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("Reporting.Dashboard", "Ordinal");
            DropColumn("Reporting.Dashboard", "Description");
        }
    }
}
