using System;
using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class ModelRefreshQueueConfiguration : EntityTypeConfiguration<ModelRefreshQueue>
    {
        public ModelRefreshQueueConfiguration()
        {
        }
        //public int Id { get; set; }
        //public int ModelId { get; set; }
        //public DateTimeOffset CreatedAt { get; set; }
        //public string CreatedBy { get; set; }
        //public DateTimeOffset? StartedAt { get; set; }
        //public DateTimeOffset? EndedAt { get; set; }
    }
}