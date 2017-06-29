using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel
{
    public class TableQueryModel
    {
        public int ModelId { get; set; }
        public string TableName { get; set; }
        public string OrderBy { get; set; }
        public bool OrderDesc { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
}