namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting15 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("Reporting.UserReportFavorite", "ReportId", "Reporting.Report");
            DropIndex("Reporting.UserReportFavorite", new[] { "ReportId" });
            CreateTable(
                "Reporting.UserFavoriteItem",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        ItemId = c.Int(nullable: false),
                        ItemType = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            DropTable("Reporting.UserReportFavorite");
        }
        
        public override void Down()
        {
            CreateTable(
                "Reporting.UserReportFavorite",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        ReportId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => new { t.UserId, t.ReportId });
            
            DropTable("Reporting.UserFavoriteItem");
            CreateIndex("Reporting.UserReportFavorite", "ReportId");
            AddForeignKey("Reporting.UserReportFavorite", "ReportId", "Reporting.Report", "Id", cascadeDelete: true);
        }
    }
}
