namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModel4 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DataModel.SourceFile",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(),
                        FilePath = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("DataModel.DataSource", "SourceFileId", c => c.Int());
            CreateIndex("DataModel.DataSource", "SourceFileId");
            AddForeignKey("DataModel.DataSource", "SourceFileId", "DataModel.SourceFile", "Id");
            DropColumn("DataModel.DataSource", "FileName");
            DropColumn("DataModel.DataSource", "FilePath");
        }
        
        public override void Down()
        {
            AddColumn("DataModel.DataSource", "FilePath", c => c.String());
            AddColumn("DataModel.DataSource", "FileName", c => c.String());
            DropForeignKey("DataModel.DataSource", "SourceFileId", "DataModel.SourceFile");
            DropIndex("DataModel.DataSource", new[] { "SourceFileId" });
            DropColumn("DataModel.DataSource", "SourceFileId");
            DropTable("DataModel.SourceFile");
        }
    }
}
