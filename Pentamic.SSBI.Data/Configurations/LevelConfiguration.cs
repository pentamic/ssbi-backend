using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class LevelConfiguration : EntityTypeConfiguration<Level>
    {
        public LevelConfiguration()
        {
            HasRequired(x => x.Hierarchy).WithMany(x => x.Levels);
        }
        //public int Id { get; set; }
        //public int HierarchyId { get; set; }
        //public int ColumnId { get; set; }
        //public string Name { get; set; }
        //public int Ordinal { get; set; }
        //public HierarchyConfiguration Hierarchy { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }
    }
}