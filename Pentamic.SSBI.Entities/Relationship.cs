using System;

namespace Pentamic.SSBI.Entities
{
    public class Relationship : IDataModelObject, IAuditable
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public int FromColumnId { get; set; }
        public int ToColumnId { get; set; }
        public bool IsActive { get; set; }
        public CrossFilteringBehavior CrossFilteringBehavior { get; set; }
        public RelationshipCardinality Cardinality { get; set; }
        public RelationshipDateBehavior? DateBehavior { get; set; }
        public SecurityFilteringBehavior SecurityFilteringBehavior { get; set; }
        //public string FromTableName { get; set; }
        //public string FromColumnName { get; set; }
        //public string ToTableName { get; set; }
        //public string ToColumnName { get; set; }

        public Column FromColumn { get; set; }
        public Column ToColumn { get; set; }
        public Model Model { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}
