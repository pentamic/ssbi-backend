using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting
{
    public class DataField
    {
        public int Id { get; set; }
        public int ReportTileId { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string TableName { get; set; }
        public string ColumnName { get; set; }
    }
}