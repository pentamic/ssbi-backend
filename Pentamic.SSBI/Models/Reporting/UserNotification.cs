using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting
{
    public class UserNotification
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int NotificationId { get; set; }
        public bool IsRead { get; set; }

        public Notification Notification { get; set; }
    }
}