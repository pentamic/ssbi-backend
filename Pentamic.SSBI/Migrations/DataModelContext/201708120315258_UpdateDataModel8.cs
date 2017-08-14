namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel8 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Model", "GenerateFromTemplate", c => c.String());
            AddColumn("DataModel.Model", "CloneFromModelId", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("DataModel.Model", "CloneFromModelId");
            DropColumn("DataModel.Model", "GenerateFromTemplate");
        }
    }
}
