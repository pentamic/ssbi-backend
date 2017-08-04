namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("DataModel.Partition", "DataSourceId", "DataModel.DataSource");
            DropIndex("DataModel.Partition", new[] { "DataSourceId" });
            AlterColumn("DataModel.Partition", "DataSourceId", c => c.Int());
            CreateIndex("DataModel.Partition", "DataSourceId");
            AddForeignKey("DataModel.Partition", "DataSourceId", "DataModel.DataSource", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("DataModel.Partition", "DataSourceId", "DataModel.DataSource");
            DropIndex("DataModel.Partition", new[] { "DataSourceId" });
            AlterColumn("DataModel.Partition", "DataSourceId", c => c.Int(nullable: false));
            CreateIndex("DataModel.Partition", "DataSourceId");
            AddForeignKey("DataModel.Partition", "DataSourceId", "DataModel.DataSource", "Id", cascadeDelete: true);
        }
    }
}
