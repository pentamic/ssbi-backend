namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel16 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Partition", "Expression", c => c.String());
            AddColumn("DataModel.Partition", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            DropColumn("DataModel.Partition", "SourceType");
        }
        
        public override void Down()
        {
            AddColumn("DataModel.Partition", "SourceType", c => c.Int(nullable: false));
            DropColumn("DataModel.Partition", "Discriminator");
            DropColumn("DataModel.Partition", "Expression");
        }
    }
}
