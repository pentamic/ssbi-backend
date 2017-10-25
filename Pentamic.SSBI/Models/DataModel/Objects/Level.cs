using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class Level : DataModelObjectBase
    {
        public int ModelId { get; set; }
        public int HierarchyId { get; set; }
        public int ColumnId { get; set; }
        public int Ordinal { get; set; }

        public Hierarchy Hierarchy { get; set; }
    }
}