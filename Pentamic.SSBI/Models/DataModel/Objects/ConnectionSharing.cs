using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class ConnectionSharing : IShareInfo
    {
        [Key]
        [Column(Order = 1)]
        public string UserId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ConnectionId { get; set; }
        public string Permission { get; set; }
        public string SharedBy { get; set; }
        public DateTimeOffset SharedAt { get; set; }
    }
}