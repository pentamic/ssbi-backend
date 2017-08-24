using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting.Query
{
    public class FilterModel
    {
        public string Name { get; set; }
        public FilterOperator FilterOperator { get; set; }
        public bool IsValue { get; set; }
        public string DataType { get; set; }
    }
}