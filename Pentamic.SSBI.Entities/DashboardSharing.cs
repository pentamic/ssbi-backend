using System;

namespace Pentamic.SSBI.Entities
{
    public class DashboardSharing : IShareInfo
    {
        public string UserId { get; set; }
        public int DashboardId { get; set; }
        public string Permission { get; set; }
        public string SharedBy { get; set; }
        public DateTimeOffset SharedAt { get; set; }

        public Dashboard Dashboard { get; set; }
    }
}