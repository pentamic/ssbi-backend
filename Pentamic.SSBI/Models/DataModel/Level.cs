using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel
{
    public class Level
    {
        public int Id { get; set; }
        public int HierarchyId { get; set; }
        public int ColumnId { get; set; }
        public string Name { get; set; }
        public int Ordinal { get; set; }
        public Hierarchy Hierarchy { get; set; }
    }
}