using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting
{
    public class NotificationSubscription
    {
        [Key]
        [Column(Order = 1)]
        public int NotificationId { get; set; }
        [Key]
        [Column(Order = 2)]
        public string UserId { get; set; }
        public bool UseEmail { get; set; }
    }
}