namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateModel6 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Model", "DefaultMode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("DataModel.Model", "DefaultMode");
        }
    }
}
