using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class Perspective : DataModelObjectBase
    {
        public int ModelId { get; set; }
        public string Description { get; set; }
    }
}