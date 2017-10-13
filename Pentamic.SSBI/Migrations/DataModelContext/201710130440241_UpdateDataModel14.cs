namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel14 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Column", "ColumnType", c => c.Int(nullable: false));
            AddColumn("DataModel.Partition", "Expression", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("DataModel.Partition", "Expression");
            DropColumn("DataModel.Column", "ColumnType");
        }
    }
}
