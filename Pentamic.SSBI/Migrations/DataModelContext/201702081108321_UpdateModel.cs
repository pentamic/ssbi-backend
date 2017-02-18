namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System.Data.Entity.Migrations;

    public partial class UpdateModel : DbMigration
    {
        public override void Up()
        {
            DropColumn("DataModel.Column", "UpdatedProperties");
            DropColumn("DataModel.Column", "OriginalValuesMap");
            DropColumn("DataModel.Relationship", "UpdatedProperties");
            DropColumn("DataModel.Model", "UpdatedProperties");
            DropColumn("DataModel.DataSource", "UpdatedProperties");
            DropColumn("DataModel.Table", "UpdatedProperties");
            DropColumn("DataModel.Measure", "UpdatedProperties");
            DropColumn("DataModel.Partition", "UpdatedProperties");
        }
        
        public override void Down()
        {
            AddColumn("DataModel.Partition", "UpdatedProperties", c => c.String());
            AddColumn("DataModel.Measure", "UpdatedProperties", c => c.String());
            AddColumn("DataModel.Table", "UpdatedProperties", c => c.String());
            AddColumn("DataModel.DataSource", "UpdatedProperties", c => c.String());
            AddColumn("DataModel.Model", "UpdatedProperties", c => c.String());
            AddColumn("DataModel.Relationship", "UpdatedProperties", c => c.String());
            AddColumn("DataModel.Column", "OriginalValuesMap", c => c.String());
            AddColumn("DataModel.Column", "UpdatedProperties", c => c.String());
        }
    }
}
