namespace Pentamic.SSBI.Models.Reporting.Query
{
    public class FieldModel
    {
        private string TableName { get; set; }
        private string ColumnName { get; set; }
        public string Name { get; set; }
        public int Ordinal { get; set; }
        public string QueryFieldType { get; set; } //Column, Measure, Aggregation

    }
}
