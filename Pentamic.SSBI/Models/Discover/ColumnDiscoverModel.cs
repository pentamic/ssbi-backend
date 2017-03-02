using Pentamic.SSBI.Models.DataModel.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Discover
{
    public class ColumnDiscoverModel
    {
        public DataSource DataSource { get; set; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }
    }
}