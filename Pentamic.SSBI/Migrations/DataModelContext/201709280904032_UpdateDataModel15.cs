namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Connection", "CreatedBy", c => c.String());
            AddColumn("DataModel.Connection", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Connection", "ModifiedBy", c => c.String());
            AddColumn("DataModel.Connection", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("DataModel.Connection", "ModifiedAt");
            DropColumn("DataModel.Connection", "ModifiedBy");
            DropColumn("DataModel.Connection", "CreatedAt");
            DropColumn("DataModel.Connection", "CreatedBy");
        }
    }
}
