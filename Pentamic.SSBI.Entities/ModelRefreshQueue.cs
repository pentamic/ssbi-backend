using System;

namespace Pentamic.SSBI.Entities
{
    public class ModelRefreshQueue
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? EndedAt { get; set; }
    }
}