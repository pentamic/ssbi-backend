﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting
{
    public class UserFavoriteDashboard : IAuditable
    {
        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int DashboardId { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public Dashboard Dashboard { get; set; }
    }
}