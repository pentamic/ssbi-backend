using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class CalculatedColumn : Column
    {
        public string Expression { get; set; }
    }
}