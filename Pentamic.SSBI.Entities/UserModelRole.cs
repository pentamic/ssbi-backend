namespace Pentamic.SSBI.Entities
{
    public class UserModelRole
    {
        public string UserId { get; set; }
        public int RoleId { get; set; }

        public ModelRole Role { get; set; }
    }
}