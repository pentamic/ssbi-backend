using System;

namespace Pentamic.SSBI.Entities
{
    public class ConnectionSharing : IShareInfo
    {
        public string UserId { get; set; }
        public int ConnectionId { get; set; }
        public string Permission { get; set; }
        public string SharedBy { get; set; }
        public DateTimeOffset SharedAt { get; set; }
    }
}