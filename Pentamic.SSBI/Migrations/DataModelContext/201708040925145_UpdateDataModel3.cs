namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel3 : DbMigration
    {
        public override void Up()
        {
            DropColumn("DataModel.Column", "State");
            DropColumn("DataModel.Relationship", "State");
            DropColumn("DataModel.Model", "State");
            DropColumn("DataModel.DataSource", "State");
            DropColumn("DataModel.Table", "State");
            DropColumn("DataModel.Hierarchy", "State");
            DropColumn("DataModel.Level", "State");
            DropColumn("DataModel.Measure", "State");
            DropColumn("DataModel.Partition", "State");
            DropColumn("DataModel.Perspective", "State");
        }
        
        public override void Down()
        {
            AddColumn("DataModel.Perspective", "State", c => c.Int(nullable: false));
            AddColumn("DataModel.Partition", "State", c => c.Int(nullable: false));
            AddColumn("DataModel.Measure", "State", c => c.Int(nullable: false));
            AddColumn("DataModel.Level", "State", c => c.Int(nullable: false));
            AddColumn("DataModel.Hierarchy", "State", c => c.Int(nullable: false));
            AddColumn("DataModel.Table", "State", c => c.Int(nullable: false));
            AddColumn("DataModel.DataSource", "State", c => c.Int(nullable: false));
            AddColumn("DataModel.Model", "State", c => c.Int(nullable: false));
            AddColumn("DataModel.Relationship", "State", c => c.Int(nullable: false));
            AddColumn("DataModel.Column", "State", c => c.Int(nullable: false));
        }
    }
}
