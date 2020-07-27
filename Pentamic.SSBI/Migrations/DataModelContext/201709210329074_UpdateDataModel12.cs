namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel12 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "DataModel.ModelProcessQueue",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ModelId = c.Int(nullable: false),
                        CreatedAt = c.DateTimeOffset(nullable: false, precision: 7),
                        CreatedBy = c.String(),
                        StartedAt = c.DateTimeOffset(precision: 7),
                        EndedAt = c.DateTimeOffset(precision: 7),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("DataModel.Model", "RefreshSchedule", c => c.String());
            AddColumn("DataModel.Model", "RefreshJobId", c => c.String());
            DropColumn("DataModel.Model", "ProcessedAt");
            DropColumn("DataModel.Model", "ProcessedBy");
            DropColumn("DataModel.Model", "IsProcessing");
        }
        
        public override void Down()
        {
            AddColumn("DataModel.Model", "IsProcessing", c => c.Boolean(nullable: false));
            AddColumn("DataModel.Model", "ProcessedBy", c => c.String());
            AddColumn("DataModel.Model", "ProcessedAt", c => c.DateTimeOffset(nullable: false, precision: 7));
            DropColumn("DataModel.Model", "RefreshJobId");
            DropColumn("DataModel.Model", "RefreshSchedule");
            DropTable("DataModel.ModelProcessQueue");
        }
    }
}
