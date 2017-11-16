using System;

namespace Pentamic.SSBI.Entities
{
    public class UserFavoriteModel : IAuditable
    {
        public string UserId { get; set; }
        public int ModelId { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }

        public Model Model { get; set; }
    }
}