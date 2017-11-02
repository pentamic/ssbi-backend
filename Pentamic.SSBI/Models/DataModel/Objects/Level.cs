using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class Level : IDataModelObject, IAuditable
    {
        public int Id { get; set; }
        public int HierarchyId { get; set; }
        public int ColumnId { get; set; }
        public string Name { get; set; }
        public int Ordinal { get; set; }
        public Hierarchy Hierarchy { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}