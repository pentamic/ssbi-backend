namespace Pentamic.SSBI.Migrations.DataModelContext
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateDataModel7 : DbMigration
    {
        public override void Up()
        {
            AddColumn("DataModel.Column", "SortByColumnId", c => c.Int());
            CreateIndex("DataModel.Column", "SortByColumnId");
            AddForeignKey("DataModel.Column", "SortByColumnId", "DataModel.Column", "Id");
            DropColumn("DataModel.Column", "SortByColumnName");
        }
        
        public override void Down()
        {
            AddColumn("DataModel.Column", "SortByColumnName", c => c.String());
            DropForeignKey("DataModel.Column", "SortByColumnId", "DataModel.Column");
            DropIndex("DataModel.Column", new[] { "SortByColumnId" });
            DropColumn("DataModel.Column", "SortByColumnId");
        }
    }
}
