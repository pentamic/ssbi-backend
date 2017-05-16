using System;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class Partition : IDataModelObject, IAuditable
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public int DataSourceId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OriginalName { get; set; }
        public string Query { get; set; }
        public DataModelObjectState State { get; set; }
        public Table Table { get; set; }
        public DataSource DataSource { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
        public DateTimeOffset RefreshedAt { get; set; }
        public string RefreshedBy { get; set; }
    }
}
