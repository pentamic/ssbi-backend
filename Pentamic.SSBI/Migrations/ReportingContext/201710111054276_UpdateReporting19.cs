namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting19 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Reporting.ReportTileRow",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportTileId = c.Int(nullable: false),
                        Code = c.String(),
                        Name = c.String(),
                        Ordinal = c.String(),
                        IsGroup = c.Boolean(nullable: false),
                        ParentId = c.Int(),
                        IsFormula = c.Boolean(nullable: false),
                        ValueExpression = c.String(),
                        FilterExpression = c.String(),
                        FormulaExpression = c.String(),
                        Indent = c.Int(nullable: false),
                        IsBold = c.Boolean(nullable: false),
                        IsItalic = c.Boolean(nullable: false),
                        IsUnderline = c.Boolean(nullable: false),
                        IsUppercase = c.Boolean(nullable: false),
                        BackgroundColor = c.String(),
                        TextColor = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Reporting.ReportTile", t => t.ReportTileId, cascadeDelete: true)
                .Index(t => t.ReportTileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Reporting.ReportTileRow", "ReportTileId", "Reporting.ReportTile");
            DropIndex("Reporting.ReportTileRow", new[] { "ReportTileId" });
            DropTable("Reporting.ReportTileRow");
        }
    }
}
