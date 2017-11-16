using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class PerspectiveConfiguration : EntityTypeConfiguration<Perspective>
    {
        public PerspectiveConfiguration() { }

        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string ModifiedBy { get; set; }
        //public DateTimeOffset ModifiedAt { get; set; }
    }
}