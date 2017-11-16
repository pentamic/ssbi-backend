﻿using System;

namespace Pentamic.SSBI.Entities
{
    public class DashboardView : IAuditable
    {
        public int Id { get; set; }
        public int DashboardId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Selections { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public Dashboard Dashboard { get; set; }
    }
}