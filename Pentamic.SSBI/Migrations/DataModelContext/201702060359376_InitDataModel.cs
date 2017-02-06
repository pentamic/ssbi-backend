namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class InitDataModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DataModel.Column",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TableId = c.Int(nullable: false),
                    Name = c.String(),
                    OriginalName = c.String(),
                    UpdatedProperties = c.String(),
                    SourceColumn = c.String(),
                    DataType = c.Int(nullable: false),
                    State = c.Int(nullable: false),
                    OriginalValuesMap = c.String(),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DataModel.Table", t => t.TableId, cascadeDelete: true)
                .Index(t => t.TableId);

            CreateTable(
                "DataModel.Relationship",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ModelId = c.Int(nullable: false),
                    Name = c.String(),
                    OriginalName = c.String(),
                    UpdatedProperties = c.String(),
                    FromColumnId = c.Int(nullable: false),
                    ToColumnId = c.Int(nullable: false),
                    State = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DataModel.Model", t => t.ModelId, cascadeDelete: true)
                .ForeignKey("DataModel.Column", t => t.FromColumnId, cascadeDelete: false)
                .ForeignKey("DataModel.Column", t => t.ToColumnId, cascadeDelete: false)
                .Index(t => t.ModelId)
                .Index(t => t.FromColumnId)
                .Index(t => t.ToColumnId);

            CreateTable(
                "DataModel.Model",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(),
                    Description = c.String(),
                    OriginalName = c.String(),
                    UpdatedProperties = c.String(),
                    DatabaseName = c.String(),
                    State = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id);

            CreateTable(
                "DataModel.DataSource",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    ModelId = c.Int(nullable: false),
                    Name = c.String(),
                    OriginalName = c.String(),
                    UpdatedProperties = c.String(),
                    Description = c.String(),
                    Type = c.Int(nullable: false),
                    Source = c.String(),
                    Catalog = c.String(),
                    User = c.String(),
                    Password = c.String(),
                    IntegratedSecurity = c.Boolean(nullable: false),
                    FileName = c.String(),
                    FilePath = c.String(),
                    State = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DataModel.Model", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);

            CreateTable(
                "DataModel.Table",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    DataSourceId = c.Int(nullable: false),
                    Name = c.String(),
                    Description = c.String(),
                    OriginalName = c.String(),
                    UpdatedProperties = c.String(),
                    SourceTable = c.String(),
                    SourceSchema = c.String(),
                    State = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DataModel.DataSource", t => t.DataSourceId, cascadeDelete: true)
                .Index(t => t.DataSourceId);

            CreateTable(
                "DataModel.Measure",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TableId = c.Int(nullable: false),
                    Name = c.String(),
                    Description = c.String(),
                    OriginalName = c.String(),
                    UpdatedProperties = c.String(),
                    Expression = c.String(),
                    State = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DataModel.Table", t => t.TableId, cascadeDelete: true)
                .Index(t => t.TableId);

            CreateTable(
                "DataModel.Partition",
                c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    TableId = c.Int(nullable: false),
                    Name = c.String(),
                    OriginalName = c.String(),
                    UpdatedProperties = c.String(),
                    Query = c.String(),
                    State = c.Int(nullable: false),
                })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DataModel.Table", t => t.TableId, cascadeDelete: true)
                .Index(t => t.TableId);

        }

        public override void Down()
        {
            DropForeignKey("DataModel.Relationship", "ToColumnId", "DataModel.Column");
            DropForeignKey("DataModel.Relationship", "FromColumnId", "DataModel.Column");
            DropForeignKey("DataModel.Relationship", "ModelId", "DataModel.Model");
            DropForeignKey("DataModel.Partition", "TableId", "DataModel.Table");
            DropForeignKey("DataModel.Measure", "TableId", "DataModel.Table");
            DropForeignKey("DataModel.Table", "DataSourceId", "DataModel.DataSource");
            DropForeignKey("DataModel.Column", "TableId", "DataModel.Table");
            DropForeignKey("DataModel.DataSource", "ModelId", "DataModel.Model");
            DropIndex("DataModel.Partition", new[] { "TableId" });
            DropIndex("DataModel.Measure", new[] { "TableId" });
            DropIndex("DataModel.Table", new[] { "DataSourceId" });
            DropIndex("DataModel.DataSource", new[] { "ModelId" });
            DropIndex("DataModel.Relationship", new[] { "ToColumnId" });
            DropIndex("DataModel.Relationship", new[] { "FromColumnId" });
            DropIndex("DataModel.Relationship", new[] { "ModelId" });
            DropIndex("DataModel.Column", new[] { "TableId" });
            DropTable("DataModel.Partition");
            DropTable("DataModel.Measure");
            DropTable("DataModel.Table");
            DropTable("DataModel.DataSource");
            DropTable("DataModel.Model");
            DropTable("DataModel.Relationship");
            DropTable("DataModel.Column");
        }
    }
}
