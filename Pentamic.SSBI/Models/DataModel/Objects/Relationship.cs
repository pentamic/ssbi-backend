using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class Relationship : DataModelObjectBase
    {
        public int ModelId { get; set; }
        public int FromTableId { get; set; }
        public int ToTableId { get; set; }
        public int FromColumnId { get; set; }
        public int ToColumnId { get; set; }
        public bool IsActive { get; set; }
        public CrossFilteringBehavior CrossFilteringBehavior { get; set; }
        public RelationshipCardinality Cardinality { get; set; }
        public RelationshipDateBehavior? DateBehavior { get; set; }
        public SecurityFilteringBehavior SecurityFilteringBehavior { get; set; }

        [ForeignKey("FromColumnId")]
        public Column FromColumn { get; set; }
        [ForeignKey("ToColumnId")]
        public Column ToColumn { get; set; }
        public Model Model { get; set; }
    }
}
