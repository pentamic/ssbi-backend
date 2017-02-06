namespace Pentamic.SSBI.Models.DataModel
{
    public class Measure : IDataModelObject
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OriginalName { get; set; }
        public string UpdatedProperties { get; set; }
        public string Expression { get; set; }
        public DataModelObjectState State { get; set; }
        public Table Table { get; set; }
    }
}
