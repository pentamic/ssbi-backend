namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel19 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Column", "ModelId", c => c.Int(nullable: false));
            AddColumn("DataModel.Relationship", "FromTableId", c => c.Int(nullable: false));
            AddColumn("DataModel.Relationship", "ToTableId", c => c.Int(nullable: false));
            AddColumn("DataModel.Partition", "ModelId", c => c.Int(nullable: false));
            AddColumn("DataModel.Hierarchy", "ModelId", c => c.Int(nullable: false));
            AddColumn("DataModel.Level", "ModelId", c => c.Int(nullable: false));
            AddColumn("DataModel.Measure", "ModelId", c => c.Int(nullable: false));
            AddColumn("DataModel.Perspective", "ModelId", c => c.Int(nullable: false));
            DropColumn("DataModel.Column", "OriginalName");
            DropColumn("DataModel.Relationship", "OriginalName");
            DropColumn("DataModel.Model", "OriginalName");
            DropColumn("DataModel.Model", "DatabaseName");
            DropColumn("DataModel.DataSource", "OriginalName");
            DropColumn("DataModel.Partition", "OriginalName");
            DropColumn("DataModel.Table", "OriginalName");
            DropColumn("DataModel.Hierarchy", "OriginalName");
            DropColumn("DataModel.Level", "OriginalName");
            DropColumn("DataModel.Measure", "OriginalName");
            DropColumn("DataModel.Role", "OriginalName");
            DropColumn("DataModel.Perspective", "OriginalName");
        }
        
        public override void Down()
        {
            AddColumn("DataModel.Perspective", "OriginalName", c => c.String());
            AddColumn("DataModel.Role", "OriginalName", c => c.String());
            AddColumn("DataModel.Measure", "OriginalName", c => c.String());
            AddColumn("DataModel.Level", "OriginalName", c => c.String());
            AddColumn("DataModel.Hierarchy", "OriginalName", c => c.String());
            AddColumn("DataModel.Table", "OriginalName", c => c.String());
            AddColumn("DataModel.Partition", "OriginalName", c => c.String());
            AddColumn("DataModel.DataSource", "OriginalName", c => c.String());
            AddColumn("DataModel.Model", "DatabaseName", c => c.String());
            AddColumn("DataModel.Model", "OriginalName", c => c.String());
            AddColumn("DataModel.Relationship", "OriginalName", c => c.String());
            AddColumn("DataModel.Column", "OriginalName", c => c.String());
            DropColumn("DataModel.Perspective", "ModelId");
            DropColumn("DataModel.Measure", "ModelId");
            DropColumn("DataModel.Level", "ModelId");
            DropColumn("DataModel.Hierarchy", "ModelId");
            DropColumn("DataModel.Partition", "ModelId");
            DropColumn("DataModel.Relationship", "ToTableId");
            DropColumn("DataModel.Relationship", "FromTableId");
            DropColumn("DataModel.Column", "ModelId");
        }
    }
}
