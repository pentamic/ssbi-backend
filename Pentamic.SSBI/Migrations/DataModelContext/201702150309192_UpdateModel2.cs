namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModel2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Partition", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("DataModel.Partition", "Description");
        }
    }
}
