namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System.Data.Entity.Migrations;

    public partial class InitReporting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Reporting.ReportPage",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportId = c.Int(nullable: false),
                        Name = c.String(),
                        Ordinal = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Reporting.Report",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Ordinal = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Reporting.ReportTile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReportPageId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        Column = c.Int(nullable: false),
                        Row = c.Int(nullable: false),
                        SizeX = c.Int(nullable: false),
                        SizeY = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("Reporting.ReportTile");
            DropTable("Reporting.Report");
            DropTable("Reporting.ReportPage");
        }
    }
}
