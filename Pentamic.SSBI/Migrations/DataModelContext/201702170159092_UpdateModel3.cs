namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModel3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DataModel.Hierarchy",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TableId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        DisplayFolder = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DataModel.Table", t => t.TableId, cascadeDelete: true)
                .Index(t => t.TableId);
            
            CreateTable(
                "DataModel.Level",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        HierarchyId = c.Int(nullable: false),
                        ColumnId = c.Int(nullable: false),
                        Name = c.String(),
                        Ordinal = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DataModel.Hierarchy", t => t.HierarchyId, cascadeDelete: true)
                .Index(t => t.HierarchyId);
            
            DropColumn("DataModel.Column", "IsKey");
        }
        
        public override void Down()
        {
            AddColumn("DataModel.Column", "IsKey", c => c.Boolean(nullable: false));
            DropForeignKey("DataModel.Hierarchy", "TableId", "DataModel.Table");
            DropForeignKey("DataModel.Level", "HierarchyId", "DataModel.Hierarchy");
            DropIndex("DataModel.Level", new[] { "HierarchyId" });
            DropIndex("DataModel.Hierarchy", new[] { "TableId" });
            DropTable("DataModel.Level");
            DropTable("DataModel.Hierarchy");
        }
    }
}
