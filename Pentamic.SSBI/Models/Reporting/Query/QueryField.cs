using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting.Query
{
    public class QueryField
    {
        private string _tableName { get; set; }
        private string _columnName { get; set; }
        public string Name { get; set; }
        public QueryFieldType Type { get; set; }
        public int Ordinal { get; set; }
        public string FilterExpression { get; set; }
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
