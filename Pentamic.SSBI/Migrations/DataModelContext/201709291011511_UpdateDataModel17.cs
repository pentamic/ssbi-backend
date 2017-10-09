namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel17 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Column", "Expression", c => c.String());
            AddColumn("DataModel.Column", "SourceColumn1", c => c.String());
            AddColumn("DataModel.Column", "Discriminator", c => c.String(nullable: false, maxLength: 128));
        }
        
        public override void Down()
        {
            DropColumn("DataModel.Column", "Discriminator");
            DropColumn("DataModel.Column", "SourceColumn1");
            DropColumn("DataModel.Column", "Expression");
        }
    }
}
