namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel13 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "DataModel.ModelProcessQueue", newName: "ModelRefreshQueue");
        }
        
        public override void Down()
        {
            RenameTable(name: "DataModel.ModelRefreshQueue", newName: "ModelProcessQueue");
        }
    }
}
