namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting5 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Reporting.Dashboard",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Reporting.DashboardTile",
                c => new
                    {
                        DashboardId = c.Int(nullable: false),
                        ReportTileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DashboardId, t.ReportTileId })
                .ForeignKey("Reporting.Dashboard", t => t.DashboardId, cascadeDelete: true)
                .ForeignKey("Reporting.ReportTile", t => t.ReportTileId, cascadeDelete: true)
                .Index(t => t.DashboardId)
                .Index(t => t.ReportTileId);
            
            CreateTable(
                "Reporting.DisplayType",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Icon = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("Reporting.ReportPage", "CreatedBy", c => c.String());
            AddColumn("Reporting.ReportPage", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Reporting.ReportPage", "ModifiedBy", c => c.String());
            AddColumn("Reporting.ReportPage", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Reporting.Report", "CreatedBy", c => c.String());
            AddColumn("Reporting.Report", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Reporting.Report", "ModifiedBy", c => c.String());
            AddColumn("Reporting.Report", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Reporting.ReportTile", "DisplayTypeId", c => c.String(maxLength: 128));
            AddColumn("Reporting.ReportTile", "CreatedBy", c => c.String());
            AddColumn("Reporting.ReportTile", "CreatedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("Reporting.ReportTile", "ModifiedBy", c => c.String());
            AddColumn("Reporting.ReportTile", "ModifiedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            CreateIndex("Reporting.ReportTile", "DisplayTypeId");
            AddForeignKey("Reporting.ReportTile", "DisplayTypeId", "Reporting.DisplayType", "Id");
            DropColumn("Reporting.ReportTile", "DisplayType");
        }
        
        public override void Down()
        {
            AddColumn("Reporting.ReportTile", "DisplayType", c => c.Int(nullable: false));
            DropForeignKey("Reporting.DashboardTile", "ReportTileId", "Reporting.ReportTile");
            DropForeignKey("Reporting.ReportTile", "DisplayTypeId", "Reporting.DisplayType");
            DropForeignKey("Reporting.DashboardTile", "DashboardId", "Reporting.Dashboard");
            DropIndex("Reporting.ReportTile", new[] { "DisplayTypeId" });
            DropIndex("Reporting.DashboardTile", new[] { "ReportTileId" });
            DropIndex("Reporting.DashboardTile", new[] { "DashboardId" });
            DropColumn("Reporting.ReportTile", "ModifiedAt");
            DropColumn("Reporting.ReportTile", "ModifiedBy");
            DropColumn("Reporting.ReportTile", "CreatedAt");
            DropColumn("Reporting.ReportTile", "CreatedBy");
            DropColumn("Reporting.ReportTile", "DisplayTypeId");
            DropColumn("Reporting.Report", "ModifiedAt");
            DropColumn("Reporting.Report", "ModifiedBy");
            DropColumn("Reporting.Report", "CreatedAt");
            DropColumn("Reporting.Report", "CreatedBy");
            DropColumn("Reporting.ReportPage", "ModifiedAt");
            DropColumn("Reporting.ReportPage", "ModifiedBy");
            DropColumn("Reporting.ReportPage", "CreatedAt");
            DropColumn("Reporting.ReportPage", "CreatedBy");
            DropTable("Reporting.DisplayType");
            DropTable("Reporting.DashboardTile");
            DropTable("Reporting.Dashboard");
        }
    }
}
