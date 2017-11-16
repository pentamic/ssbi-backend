using System.Collections.Generic;

namespace Pentamic.SSBI.Services
{
    public class TableDetailResult
    {
        public List<dynamic> Data { get; set; }
        public List<ColumnDiscoverResult> Columns { get; set; }
    }
}