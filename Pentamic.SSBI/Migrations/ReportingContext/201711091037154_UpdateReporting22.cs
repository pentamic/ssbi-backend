namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting22 : DbMigration
    {
        public override void Up()
        {
            AddColumn("Reporting.ReportFilter", "DefaultValue", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("Reporting.ReportFilter", "DefaultValue");
        }
    }
}
