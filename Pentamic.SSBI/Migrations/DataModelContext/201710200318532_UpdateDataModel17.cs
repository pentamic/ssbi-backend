namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel17 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("DataModel.Relationship", "DateBehavior", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("DataModel.Relationship", "DateBehavior", c => c.Int(nullable: false));
        }
    }
}
