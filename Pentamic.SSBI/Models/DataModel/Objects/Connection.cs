using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class Connection
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ConnectionType Type { get; set; }
    }
}