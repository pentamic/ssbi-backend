using System.Data.Entity.ModelConfiguration;
using Pentamic.SSBI.Entities;

namespace Pentamic.SSBI.Data.Configurations
{
    public class NotificationSubscriptionConfiguration : EntityTypeConfiguration<NotificationSubscription>
    {
        public NotificationSubscriptionConfiguration()
        {
            HasKey(x => new { x.NotificationId, x.UserId });
        }
        //public int NotificationId { get; set; }
        //public string UserId { get; set; }
        //public bool UseEmail { get; set; }
    }
}