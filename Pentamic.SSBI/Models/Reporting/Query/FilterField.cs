namespace Pentamic.SSBI.Models.Reporting.Query
{
    public class FilterField
    {
        private string _tableName { get; set; }
        private string _columnName { get; set; }
        public string Name { get; set; }
        public FilterOperator Operator { get; set; }
        public string Value { get; set; }
        public bool IsMeasure { get; set; }
        public string TableName
        {
            get
            {
                if (_tableName != null)
                {
                    return _tableName;
                }
                var tsIdx = Name.IndexOf('\'');
                var teIdx = Name.LastIndexOf('\'');
                _tableName = Name.Substring(tsIdx, teIdx + 1 - tsIdx);
                return _tableName;
            }
        }
        public string ColumnName
        {
            get
            {
                if (_columnName != null)
                {
                    return _columnName;
                }
                var csIdx = Name.IndexOf('[');
                var ceIdx = Name.Length;
                _columnName = Name.Substring(csIdx, ceIdx - csIdx);
                return _columnName;
            }
        }
    }
}