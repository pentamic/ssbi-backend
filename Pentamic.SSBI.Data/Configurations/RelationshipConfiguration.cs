using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class RelationshipConfiguration : EntityTypeConfiguration<Relationship>
    {
        public RelationshipConfiguration()
        {
            HasRequired(x => x.Model).WithMany(x => x.Relationships);
            HasRequired(x => x.FromColumn).WithMany(x => x.RelationshipFrom)
                .HasForeignKey(x => x.FromColumnId).WillCascadeOnDelete(false);
            HasRequired(x => x.ToColumn).WithMany(x => x.RelationshipTo)
                .HasForeignKey(x => x.ToColumnId).WillCascadeOnDelete(false);
        }

        //public int Id { get; set; }
        //public int ModelId { get; set; }
        //public string Name { get; set; }
        //public int FromColumnId { get; set; }
        //public int ToColumnId { get; set; }
        //public bool IsActive { get; set; }
        //public CrossFilteringBehavior CrossFilteringBehavior { get; set; }
        //public RelationshipCardinality Cardinality { get; set; }
        //public RelationshipDateBehavior? DateBehavior { get; set; }
        //public SecurityFilteringBehavior SecurityFilteringBehavior { get; set; }
        //public string FromTableName { get; set; }
        //public string FromColumnName { get; set; }
        //public string ToTableName { get; set; }
        //public string ToColumnName { get; set; }
        //public ColumnConfiguration FromColumn { get; set; }
        //public ColumnConfiguration ToColumn { get; set; }
        //public ModelConfiguration Model { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }
    }
}
