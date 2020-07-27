namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel15 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Column", "IsKey", c => c.Boolean(nullable: false));
            AddColumn("DataModel.Table", "DataCategory", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("DataModel.Table", "DataCategory");
            DropColumn("DataModel.Column", "IsKey");
        }
    }
}
