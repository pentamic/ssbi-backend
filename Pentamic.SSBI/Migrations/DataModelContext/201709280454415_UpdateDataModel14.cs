namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel14 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("DataModel.DataSource", "SourceFileId", "DataModel.SourceFile");
            DropIndex("DataModel.DataSource", new[] { "SourceFileId" });
            CreateTable(
                "DataModel.Connection",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SourceFileId = c.Int(),
                        Server = c.String(),
                        Database = c.String(),
                        IntegratedSecurity = c.Boolean(),
                        User = c.String(),
                        Password = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("DataModel.SourceFile", t => t.SourceFileId, cascadeDelete: true)
                .Index(t => t.SourceFileId);
            
            CreateTable(
                "DataModel.ModelConnection",
                c => new
                    {
                        ModelId = c.Int(nullable: false),
                        ConnectionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ModelId, t.ConnectionId })
                .ForeignKey("DataModel.Connection", t => t.ConnectionId, cascadeDelete: true)
                .ForeignKey("DataModel.Model", t => t.ModelId, cascadeDelete: true)
                .Index(t => t.ModelId)
                .Index(t => t.ConnectionId);
            
            AddColumn("DataModel.DataSource", "ConnectionId", c => c.Int(nullable: false));
            CreateIndex("DataModel.DataSource", "ConnectionId");
            AddForeignKey("DataModel.DataSource", "ConnectionId", "DataModel.Connection", "Id", cascadeDelete: true);
            DropColumn("DataModel.DataSource", "Type");
            DropColumn("DataModel.DataSource", "Source");
            DropColumn("DataModel.DataSource", "Catalog");
            DropColumn("DataModel.DataSource", "User");
            DropColumn("DataModel.DataSource", "Password");
            DropColumn("DataModel.DataSource", "IntegratedSecurity");
            DropColumn("DataModel.DataSource", "SourceFileId");
        }
        
        public override void Down()
        {
            AddColumn("DataModel.DataSource", "SourceFileId", c => c.Int());
            AddColumn("DataModel.DataSource", "IntegratedSecurity", c => c.Boolean(nullable: false));
            AddColumn("DataModel.DataSource", "Password", c => c.String());
            AddColumn("DataModel.DataSource", "User", c => c.String());
            AddColumn("DataModel.DataSource", "Catalog", c => c.String());
            AddColumn("DataModel.DataSource", "Source", c => c.String());
            AddColumn("DataModel.DataSource", "Type", c => c.Int(nullable: false));
            DropForeignKey("DataModel.DataSource", "ConnectionId", "DataModel.Connection");
            DropForeignKey("DataModel.Connection", "SourceFileId", "DataModel.SourceFile");
            DropForeignKey("DataModel.ModelConnection", "ModelId", "DataModel.Model");
            DropForeignKey("DataModel.ModelConnection", "ConnectionId", "DataModel.Connection");
            DropIndex("DataModel.ModelConnection", new[] { "ConnectionId" });
            DropIndex("DataModel.ModelConnection", new[] { "ModelId" });
            DropIndex("DataModel.Connection", new[] { "SourceFileId" });
            DropIndex("DataModel.DataSource", new[] { "ConnectionId" });
            DropColumn("DataModel.DataSource", "ConnectionId");
            DropTable("DataModel.ModelConnection");
            DropTable("DataModel.Connection");
            CreateIndex("DataModel.DataSource", "SourceFileId");
            AddForeignKey("DataModel.DataSource", "SourceFileId", "DataModel.SourceFile", "Id");
        }
    }
}
