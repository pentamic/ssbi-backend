using Pentamic.SSBI.Models.DataModel.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Discover
{
    public class DataDiscoverModel
    {
        public int DataSourceId { get; set; }
        public string TableSchema { get; set; }
        public string TableName { get; set; }
    }
}