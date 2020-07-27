namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel18 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Column", "Expression", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("DataModel.Column", "Expression");
        }
    }
}
