using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Services.SSAS.Metadata
{
    public class DateTableCreateModel
    {
        public int ModelId { get; set; }
        public string TableName { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
    }
}