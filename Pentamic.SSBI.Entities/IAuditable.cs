using System;

namespace Pentamic.SSBI.Entities
{
    public interface IAuditable
    {
        string CreatedBy { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        string ModifiedBy { get; set; }
        DateTimeOffset ModifiedAt { get; set; }
    }
}
