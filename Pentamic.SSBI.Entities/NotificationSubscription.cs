namespace Pentamic.SSBI.Entities
{
    public class NotificationSubscription
    {
        public int NotificationId { get; set; }
        public string UserId { get; set; }
        public bool UseEmail { get; set; }
    }
}