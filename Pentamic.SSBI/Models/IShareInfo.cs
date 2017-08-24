using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models
{
    public interface IShareInfo
    {
        string SharedBy { get; set; }
        DateTimeOffset SharedAt { get; set; }
    }
}