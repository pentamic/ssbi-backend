using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class UserNotificationConfiguration : EntityTypeConfiguration<UserNotification>
    {
        public UserNotificationConfiguration()
        {
            HasRequired(x => x.Notification);
        }
        //public int Id { get; set; }
        //public string UserId { get; set; }
        //public int NotificationId { get; set; }
        //public bool IsRead { get; set; }

        //public NotificationConfiguration Notification { get; set; }
    }
}