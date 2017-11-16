using System;

namespace Pentamic.SSBI.Entities
{
    public class Measure : IDataModelObject, IAuditable
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayFolder { get; set; }
        public string Expression { get; set; }
        public string FormatString { get; set; }
        public Table Table { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}
