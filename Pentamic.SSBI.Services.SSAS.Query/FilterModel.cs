namespace Pentamic.SSBI.Services.SSAS.Query
{
    public class FilterModel
    {
        public string Name { get; set; }
        public FilterOperator FilterOperator { get; set; }
        public bool IsValue { get; set; }
        public string DataType { get; set; }
    }
}