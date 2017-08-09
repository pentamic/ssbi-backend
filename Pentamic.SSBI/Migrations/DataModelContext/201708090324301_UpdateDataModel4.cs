namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Model", "ProcessedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Model", "ProcessedBy", c => c.String());
            AddColumn("DataModel.Model", "IsProcessing", c => c.Boolean(nullable: false));
            AddColumn("DataModel.Table", "ProcessedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Table", "ProcessedBy", c => c.String());
            AddColumn("DataModel.Table", "IsProcessing", c => c.Boolean(nullable: false));
            AddColumn("DataModel.Partition", "ProcessedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Partition", "ProcessedBy", c => c.String());
            AddColumn("DataModel.Partition", "IsProcessing", c => c.Boolean(nullable: false));
            DropColumn("DataModel.Model", "RefreshedBy");
            DropColumn("DataModel.Model", "IsLocked");
            DropColumn("DataModel.Table", "RefreshedAt");
            DropColumn("DataModel.Table", "RefreshedBy");
            DropColumn("DataModel.Partition", "RefreshedAt");
            DropColumn("DataModel.Partition", "RefreshedBy");
        }
        
        public override void Down()
        {
            AddColumn("DataModel.Partition", "RefreshedBy", c => c.String());
            AddColumn("DataModel.Partition", "RefreshedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Table", "RefreshedBy", c => c.String());
            AddColumn("DataModel.Table", "RefreshedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            AddColumn("DataModel.Model", "IsLocked", c => c.Boolean(nullable: false));
            AddColumn("DataModel.Model", "RefreshedBy", c => c.String());
            DropColumn("DataModel.Partition", "IsProcessing");
            DropColumn("DataModel.Partition", "ProcessedBy");
            DropColumn("DataModel.Partition", "ProcessedAt");
            DropColumn("DataModel.Table", "IsProcessing");
            DropColumn("DataModel.Table", "ProcessedBy");
            DropColumn("DataModel.Table", "ProcessedAt");
            DropColumn("DataModel.Model", "IsProcessing");
            DropColumn("DataModel.Model", "ProcessedBy");
            DropColumn("DataModel.Model", "ProcessedAt");
        }
    }
}
