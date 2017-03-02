namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Column", "CreatedBy", c => c.String());
            AddColumn("DataModel.Column", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Column", "ModifiedBy", c => c.String());
            AddColumn("DataModel.Column", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Relationship", "CreatedBy", c => c.String());
            AddColumn("DataModel.Relationship", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Relationship", "ModifiedBy", c => c.String());
            AddColumn("DataModel.Relationship", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Model", "CreatedBy", c => c.String());
            AddColumn("DataModel.Model", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Model", "ModifiedBy", c => c.String());
            AddColumn("DataModel.Model", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Model", "RefreshedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Model", "RefreshedBy", c => c.String());
            AddColumn("DataModel.Model", "IsLocked", c => c.Boolean(nullable: false));
            AddColumn("DataModel.DataSource", "CreatedBy", c => c.String());
            AddColumn("DataModel.DataSource", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.DataSource", "ModifiedBy", c => c.String());
            AddColumn("DataModel.DataSource", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.SourceFile", "CreatedBy", c => c.String());
            AddColumn("DataModel.SourceFile", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.SourceFile", "ModifiedBy", c => c.String());
            AddColumn("DataModel.SourceFile", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Table", "CreatedBy", c => c.String());
            AddColumn("DataModel.Table", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Table", "ModifiedBy", c => c.String());
            AddColumn("DataModel.Table", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Table", "RefreshedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Table", "RefreshedBy", c => c.String());
            AddColumn("DataModel.Hierarchy", "OriginalName", c => c.String());
            AddColumn("DataModel.Hierarchy", "State", c => c.Int(nullable: false));
            AddColumn("DataModel.Hierarchy", "CreatedBy", c => c.String());
            AddColumn("DataModel.Hierarchy", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Hierarchy", "ModifiedBy", c => c.String());
            AddColumn("DataModel.Hierarchy", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Level", "OriginalName", c => c.String());
            AddColumn("DataModel.Level", "State", c => c.Int(nullable: false));
            AddColumn("DataModel.Level", "CreatedBy", c => c.String());
            AddColumn("DataModel.Level", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Level", "ModifiedBy", c => c.String());
            AddColumn("DataModel.Level", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Measure", "CreatedBy", c => c.String());
            AddColumn("DataModel.Measure", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Measure", "ModifiedBy", c => c.String());
            AddColumn("DataModel.Measure", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Partition", "CreatedBy", c => c.String());
            AddColumn("DataModel.Partition", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Partition", "ModifiedBy", c => c.String());
            AddColumn("DataModel.Partition", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Partition", "RefreshedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Partition", "RefreshedBy", c => c.String());
            AddColumn("DataModel.Perspective", "OriginalName", c => c.String());
            AddColumn("DataModel.Perspective", "State", c => c.Int(nullable: false));
            AddColumn("DataModel.Perspective", "CreatedBy", c => c.String());
            AddColumn("DataModel.Perspective", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Perspective", "ModifiedBy", c => c.String());
            AddColumn("DataModel.Perspective", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
        }
        
        public override void Down()
        {
            DropColumn("DataModel.Perspective", "ModifiedAt");
            DropColumn("DataModel.Perspective", "ModifiedBy");
            DropColumn("DataModel.Perspective", "CreatedAt");
            DropColumn("DataModel.Perspective", "CreatedBy");
            DropColumn("DataModel.Perspective", "State");
            DropColumn("DataModel.Perspective", "OriginalName");
            DropColumn("DataModel.Partition", "RefreshedBy");
            DropColumn("DataModel.Partition", "RefreshedAt");
            DropColumn("DataModel.Partition", "ModifiedAt");
            DropColumn("DataModel.Partition", "ModifiedBy");
            DropColumn("DataModel.Partition", "CreatedAt");
            DropColumn("DataModel.Partition", "CreatedBy");
            DropColumn("DataModel.Measure", "ModifiedAt");
            DropColumn("DataModel.Measure", "ModifiedBy");
            DropColumn("DataModel.Measure", "CreatedAt");
            DropColumn("DataModel.Measure", "CreatedBy");
            DropColumn("DataModel.Level", "ModifiedAt");
            DropColumn("DataModel.Level", "ModifiedBy");
            DropColumn("DataModel.Level", "CreatedAt");
            DropColumn("DataModel.Level", "CreatedBy");
            DropColumn("DataModel.Level", "State");
            DropColumn("DataModel.Level", "OriginalName");
            DropColumn("DataModel.Hierarchy", "ModifiedAt");
            DropColumn("DataModel.Hierarchy", "ModifiedBy");
            DropColumn("DataModel.Hierarchy", "CreatedAt");
            DropColumn("DataModel.Hierarchy", "CreatedBy");
            DropColumn("DataModel.Hierarchy", "State");
            DropColumn("DataModel.Hierarchy", "OriginalName");
            DropColumn("DataModel.Table", "RefreshedBy");
            DropColumn("DataModel.Table", "RefreshedAt");
            DropColumn("DataModel.Table", "ModifiedAt");
            DropColumn("DataModel.Table", "ModifiedBy");
            DropColumn("DataModel.Table", "CreatedAt");
            DropColumn("DataModel.Table", "CreatedBy");
            DropColumn("DataModel.SourceFile", "ModifiedAt");
            DropColumn("DataModel.SourceFile", "ModifiedBy");
            DropColumn("DataModel.SourceFile", "CreatedAt");
            DropColumn("DataModel.SourceFile", "CreatedBy");
            DropColumn("DataModel.DataSource", "ModifiedAt");
            DropColumn("DataModel.DataSource", "ModifiedBy");
            DropColumn("DataModel.DataSource", "CreatedAt");
            DropColumn("DataModel.DataSource", "CreatedBy");
            DropColumn("DataModel.Model", "IsLocked");
            DropColumn("DataModel.Model", "RefreshedBy");
            DropColumn("DataModel.Model", "RefreshedAt");
            DropColumn("DataModel.Model", "ModifiedAt");
            DropColumn("DataModel.Model", "ModifiedBy");
            DropColumn("DataModel.Model", "CreatedAt");
            DropColumn("DataModel.Model", "CreatedBy");
            DropColumn("DataModel.Relationship", "ModifiedAt");
            DropColumn("DataModel.Relationship", "ModifiedBy");
            DropColumn("DataModel.Relationship", "CreatedAt");
            DropColumn("DataModel.Relationship", "CreatedBy");
            DropColumn("DataModel.Column", "ModifiedAt");
            DropColumn("DataModel.Column", "ModifiedBy");
            DropColumn("DataModel.Column", "CreatedAt");
            DropColumn("DataModel.Column", "CreatedBy");
        }
    }
}
