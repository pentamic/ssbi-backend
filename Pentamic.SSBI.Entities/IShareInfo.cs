using System;

namespace Pentamic.SSBI.Entities
{
    public interface IShareInfo
    {
        string SharedBy { get; set; }
        DateTimeOffset SharedAt { get; set; }
    }
}