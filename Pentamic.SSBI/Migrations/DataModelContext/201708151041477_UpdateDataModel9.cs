namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel9 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DataModel.Role",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelId = c.Int(nullable: false),
                        Name = c.String(),
                        OriginalName = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DataModel.Model", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);
            
            CreateTable(
                "DataModel.RoleTablePermission",
                c => new
                    {
                        RoleId = c.Int(nullable: false),
                        TableId = c.Int(nullable: false),
                        FilterExpression = c.String(),
                    })
                .PrimaryKey(t => new { t.RoleId, t.TableId })
                .ForeignKey("DataModel.Role", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("DataModel.Table", t => t.TableId, cascadeDelete: false)
                .Index(t => t.RoleId)
                .Index(t => t.TableId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DataModel.RoleTablePermission", "TableId", "DataModel.Table");
            DropForeignKey("DataModel.RoleTablePermission", "RoleId", "DataModel.Role");
            DropForeignKey("DataModel.Role", "ModelId", "DataModel.Model");
            DropIndex("DataModel.RoleTablePermission", new[] { "TableId" });
            DropIndex("DataModel.RoleTablePermission", new[] { "RoleId" });
            DropIndex("DataModel.Role", new[] { "ModelId" });
            DropTable("DataModel.RoleTablePermission");
            DropTable("DataModel.Role");
        }
    }
}
