namespace Pentamic.SSBI.Migrations.ReportingContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateReporting23 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Reporting.Alert",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                        MainValueField = c.String(),
                        MainValueModification = c.String(),
                        MainFilterExpression = c.String(),
                        MainCustomExpression = c.String(),
                        TargetValueField = c.String(),
                        TargetValueModification = c.String(),
                        TargetFilterExpression = c.String(),
                        TargetCustomExpression = c.String(),
                        Condition = c.Int(nullable: false),
                        UseThresold = c.Boolean(nullable: false),
                        Thresold = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Frequency = c.Int(nullable: false),
                        LastRun = c.DateTimeOffset(nullable: false, precision: 7),
                        IsActive = c.Boolean(nullable: false),
                        NotificationId = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Reporting.Notification", t => t.NotificationId, cascadeDelete: true)
                .Index(t => t.NotificationId);
            
            CreateTable(
                "Reporting.Notification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Content = c.String(),
                        CreatedBy = c.String(),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        ModifiedBy = c.String(),
                        ModifiedAt = c.DateTimeOffset(nullable: false, precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "Reporting.NotificationSubscription",
                c => new
                    {
                        NotificationId = c.Int(nullable: false),
                        UserId = c.String(nullable: false, maxLength: 128),
                        UseEmail = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.NotificationId, t.UserId });
            
            CreateTable(
                "Reporting.UserNotification",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(),
                        NotificationId = c.Int(nullable: false),
                        IsRead = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("Reporting.Notification", t => t.NotificationId, cascadeDelete: true)
                .Index(t => t.NotificationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("Reporting.UserNotification", "NotificationId", "Reporting.Notification");
            DropForeignKey("Reporting.Alert", "NotificationId", "Reporting.Notification");
            DropIndex("Reporting.UserNotification", new[] { "NotificationId" });
            DropIndex("Reporting.Alert", new[] { "NotificationId" });
            DropTable("Reporting.UserNotification");
            DropTable("Reporting.NotificationSubscription");
            DropTable("Reporting.Notification");
            DropTable("Reporting.Alert");
        }
    }
}
