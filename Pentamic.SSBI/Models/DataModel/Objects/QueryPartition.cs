using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class QueryPartition : Partition
    {
        public int? DataSourceId { get; set; }
        public string Query { get; set; }
        public DataSource DataSource { get; set; }
    }
}
