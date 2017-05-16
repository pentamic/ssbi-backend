namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class UpdateModel5 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("DataModel.Table", "DataSourceId", "DataModel.DataSource");
            DropIndex("DataModel.Table", new[] { "DataSourceId" });
            RenameColumn("DataModel.Table", "DataSourceId", "ModelId");
            AddColumn("DataModel.Partition", "DataSourceId", c => c.Int(nullable: false));
            CreateIndex("DataModel.Table", "ModelId");
            CreateIndex("DataModel.Partition", "DataSourceId");
            AddForeignKey("DataModel.Table", "ModelId", "DataModel.Model", "Id", cascadeDelete: true);
            AddForeignKey("DataModel.Partition", "DataSourceId", "DataModel.DataSource", "Id", cascadeDelete: false);
        }

        public override void Down()
        {
            AddColumn("DataModel.Table", "DataSourceId", c => c.Int(nullable: false));
            DropForeignKey("DataModel.Partition", "DataSourceId", "DataModel.DataSource");
            DropForeignKey("DataModel.Table", "ModelId", "DataModel.Model");
            DropIndex("DataModel.Partition", new[] { "DataSourceId" });
            DropIndex("DataModel.Table", new[] { "ModelId" });
            DropColumn("DataModel.Partition", "DataSourceId");
            DropColumn("DataModel.Table", "ModelId");
            CreateIndex("DataModel.Table", "DataSourceId");
            AddForeignKey("DataModel.Table", "DataSourceId", "DataModel.DataSource", "Id", cascadeDelete: true);
        }
    }
}
