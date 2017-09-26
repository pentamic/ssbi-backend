using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel.Connections
{
    public class DatabaseConnection : Connection
    {
        public string Server { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}