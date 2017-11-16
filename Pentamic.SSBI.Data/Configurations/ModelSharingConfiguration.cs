using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class ModelSharingConfiguration : EntityTypeConfiguration<ModelSharing>
    {
        public ModelSharingConfiguration()
        {
            HasKey(x => new { x.UserId, x.ModelId });
            HasRequired(x => x.Model).WithMany(x => x.ModelSharings);
        }
        //public string UserId { get; set; }
        //public int ModelId { get; set; }
        //public string Permission { get; set; }
        //public string SharedBy { get; set; }
        //public DateTimeOffset SharedAt { get; set; }

        //public ModelConfiguration Model { get; set; }
    }
}