namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DataModel.UserFavoriteModel",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ModelId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.ModelId })
                .ForeignKey("DataModel.Model", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);
            
            CreateTable(
                "DataModel.UserModelActivity",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ModelId = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedBy = c.String(),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DataModel.Model", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DataModel.UserModelActivity", "ModelId", "DataModel.Model");
            DropForeignKey("DataModel.UserFavoriteModel", "ModelId", "DataModel.Model");
            DropIndex("DataModel.UserModelActivity", new[] { "ModelId" });
            DropIndex("DataModel.UserFavoriteModel", new[] { "ModelId" });
            DropTable("DataModel.UserModelActivity");
            DropTable("DataModel.UserFavoriteModel");
        }
    }
}
