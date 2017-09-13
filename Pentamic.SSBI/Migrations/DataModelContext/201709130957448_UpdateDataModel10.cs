namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel10 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DataModel.ModelSharing",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ModelId = c.Int(nullable: false),
                        Permission = c.String(),
                        SharedBy = c.String(),
                        SharedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.ModelId })
                .ForeignKey("DataModel.Model", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("DataModel.ModelSharing", "ModelId", "DataModel.Model");
            DropIndex("DataModel.ModelSharing", new[] { "ModelId" });
            DropTable("DataModel.ModelSharing");
        }
    }
}
