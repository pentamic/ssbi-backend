namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.DataSource", "ConnectionString", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("DataModel.DataSource", "ConnectionString");
        }
    }
}
