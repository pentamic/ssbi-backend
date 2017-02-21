using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Discover
{
    public class RelationshipDiscoverResult
    {
        public string PkTableSchema { get; set; }
        public string PkTableName { get; set; }
        public string FkTableSchema { get; set; }
        public string FkTableName { get; set; }
    }
}