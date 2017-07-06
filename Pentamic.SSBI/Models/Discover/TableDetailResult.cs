using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Discover
{
    public class TableDetailResult
    {
        public List<dynamic> Data { get; set; }
        public List<ColumnDiscoverResult> Columns { get; set; }
    }
}