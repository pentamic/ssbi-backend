using System;

namespace Pentamic.SSBI.Models.Reporting
{
    public class DashboardFilter : IAuditable
    {
        public int Id { get; set; }
        public int DashboardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FilterType { get; set; }
        public string DataConfig { get; set; }

        public Dashboard Dashboard { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}