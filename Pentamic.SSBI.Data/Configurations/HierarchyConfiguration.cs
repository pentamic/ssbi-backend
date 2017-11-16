using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class HierarchyConfiguration : EntityTypeConfiguration<Hierarchy>
    {
        public HierarchyConfiguration()
        {
            HasRequired(x => x.Table).WithMany(x => x.Hierarchies);
        }
        //public int Id { get; set; }
        //public int TableId { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string DisplayFolder { get; set; }
        //public List<LevelConfiguration> Levels { get; set; }
        //public TableConfiguration Table { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }
    }
}