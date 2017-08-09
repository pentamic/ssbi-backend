using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel
{
    public class ModelRefreshModel
    {
        public int ModelId { get; set; }
        public List<string> TableNames { get; set; }
    }
}