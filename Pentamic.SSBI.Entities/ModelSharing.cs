using System;

namespace Pentamic.SSBI.Entities
{
    public class ModelSharing : IShareInfo
    {
        public string UserId { get; set; }
        public int ModelId { get; set; }
        public string Permission { get; set; }
        public string SharedBy { get; set; }
        public DateTimeOffset SharedAt { get; set; }

        public Model Model { get; set; }
    }
}