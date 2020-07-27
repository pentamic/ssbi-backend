namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Relationship", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("DataModel.Relationship", "CrossFilteringBehavior", c => c.Int(nullable: false));
            AddColumn("DataModel.Relationship", "Cardinality", c => c.Int(nullable: false));
            AddColumn("DataModel.Relationship", "DateBehavior", c => c.Int(nullable: false));
            AddColumn("DataModel.Relationship", "SecurityFilteringBehavior", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("DataModel.Relationship", "SecurityFilteringBehavior");
            DropColumn("DataModel.Relationship", "DateBehavior");
            DropColumn("DataModel.Relationship", "Cardinality");
            DropColumn("DataModel.Relationship", "CrossFilteringBehavior");
            DropColumn("DataModel.Relationship", "IsActive");
        }
    }
}
