namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel19 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DataModel.UserRole",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("DataModel.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            AddColumn("DataModel.Role", "Description", c => c.String());
            AddColumn("DataModel.Role", "ModelPermission", c => c.Int(nullable: false));
            AddColumn("DataModel.RoleTablePermission", "MetadataPermission", c => c.Boolean(nullable: false));
            DropColumn("DataModel.Column", "OriginalName");
            DropColumn("DataModel.Relationship", "OriginalName");
            DropColumn("DataModel.Model", "OriginalName");
            DropColumn("DataModel.Model", "DatabaseName");
            DropColumn("DataModel.DataSource", "OriginalName");
            DropColumn("DataModel.Partition", "OriginalName");
            DropColumn("DataModel.Table", "OriginalName");
            DropColumn("DataModel.Hierarchy", "OriginalName");
            DropColumn("DataModel.Level", "OriginalName");
            DropColumn("DataModel.Measure", "OriginalName");
            DropColumn("DataModel.Role", "OriginalName");
            DropColumn("DataModel.Perspective", "OriginalName");
        }
        
        public override void Down()
        {
            AddColumn("DataModel.Perspective", "OriginalName", c => c.String());
            AddColumn("DataModel.Role", "OriginalName", c => c.String());
            AddColumn("DataModel.Measure", "OriginalName", c => c.String());
            AddColumn("DataModel.Level", "OriginalName", c => c.String());
            AddColumn("DataModel.Hierarchy", "OriginalName", c => c.String());
            AddColumn("DataModel.Table", "OriginalName", c => c.String());
            AddColumn("DataModel.Partition", "OriginalName", c => c.String());
            AddColumn("DataModel.DataSource", "OriginalName", c => c.String());
            AddColumn("DataModel.Model", "DatabaseName", c => c.String());
            AddColumn("DataModel.Model", "OriginalName", c => c.String());
            AddColumn("DataModel.Relationship", "OriginalName", c => c.String());
            AddColumn("DataModel.Column", "OriginalName", c => c.String());
            DropForeignKey("DataModel.UserRole", "RoleId", "DataModel.Role");
            DropIndex("DataModel.UserRole", new[] { "RoleId" });
            DropColumn("DataModel.RoleTablePermission", "MetadataPermission");
            DropColumn("DataModel.Role", "ModelPermission");
            DropColumn("DataModel.Role", "Description");
            DropTable("DataModel.UserRole");
        }
    }
}
