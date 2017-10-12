using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting.Query
{
    public class QueryModel2
    {
        public int TileId { get; set; }
        public DateTime Date { get; set; }
        public string FilterExpression { get; set; }
    }
}