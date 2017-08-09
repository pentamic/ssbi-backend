namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel5 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Partition", "SourceType", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("DataModel.Partition", "SourceType");
        }
    }
}
