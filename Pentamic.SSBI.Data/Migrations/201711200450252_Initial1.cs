namespace Pentamic.SSBI.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ReportTile", "DisplayTypeId", "dbo.DisplayType");
            DropIndex("dbo.ReportTile", new[] { "DisplayTypeId" });
            AlterColumn("dbo.ReportTile", "DisplayTypeId", c => c.String(maxLength: 128));
            CreateIndex("dbo.ReportTile", "DisplayTypeId");
            AddForeignKey("dbo.ReportTile", "DisplayTypeId", "dbo.DisplayType", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ReportTile", "DisplayTypeId", "dbo.DisplayType");
            DropIndex("dbo.ReportTile", new[] { "DisplayTypeId" });
            AlterColumn("dbo.ReportTile", "DisplayTypeId", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.ReportTile", "DisplayTypeId");
            AddForeignKey("dbo.ReportTile", "DisplayTypeId", "dbo.DisplayType", "Id", cascadeDelete: true);
        }
    }
}
