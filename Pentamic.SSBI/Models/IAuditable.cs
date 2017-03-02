using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentamic.SSBI.Models
{
    interface IAuditable
    {
        string CreatedBy { get; set; }
        DateTimeOffset CreatedAt { get; set; }
        string ModifiedBy { get; set; }
        DateTimeOffset ModifiedAt { get; set; }
    }
}
