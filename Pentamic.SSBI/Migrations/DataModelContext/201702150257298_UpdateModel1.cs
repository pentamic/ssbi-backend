namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModel1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DataModel.PerspectiveColumn",
                c => new
                    {
                        PerspectiveId = c.Int(nullable: false),
                        ColumnId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.PerspectiveId, t.ColumnId })
                .ForeignKey("DataModel.Column", t => t.ColumnId, cascadeDelete: true)
                .ForeignKey("DataModel.Perspective", t => t.PerspectiveId, cascadeDelete: true)
                .Index(t => t.PerspectiveId)
                .Index(t => t.ColumnId);
            
            CreateTable(
                "DataModel.Perspective",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("DataModel.Column", "Description", c => c.String());
            AddColumn("DataModel.Column", "DisplayFolder", c => c.String());
            AddColumn("DataModel.Column", "FormatString", c => c.String());
            AddColumn("DataModel.Column", "IsKey", c => c.Boolean(nullable: false));
            AddColumn("DataModel.Column", "IsHidden", c => c.Boolean(nullable: false));
            AddColumn("DataModel.Measure", "DisplayFolder", c => c.String());
            AddColumn("DataModel.Measure", "FormatString", c => c.String());
        }
        
        public override void Down()
        {
            DropForeignKey("DataModel.PerspectiveColumn", "PerspectiveId", "DataModel.Perspective");
            DropForeignKey("DataModel.PerspectiveColumn", "ColumnId", "DataModel.Column");
            DropIndex("DataModel.PerspectiveColumn", new[] { "ColumnId" });
            DropIndex("DataModel.PerspectiveColumn", new[] { "PerspectiveId" });
            DropColumn("DataModel.Measure", "FormatString");
            DropColumn("DataModel.Measure", "DisplayFolder");
            DropColumn("DataModel.Column", "IsHidden");
            DropColumn("DataModel.Column", "IsKey");
            DropColumn("DataModel.Column", "FormatString");
            DropColumn("DataModel.Column", "DisplayFolder");
            DropColumn("DataModel.Column", "Description");
            DropTable("DataModel.Perspective");
            DropTable("DataModel.PerspectiveColumn");
        }
    }
}
