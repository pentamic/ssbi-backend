using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class RoleTablePermission
    {
        [Key]
        [Column(Order = 1)]
        public int RoleId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int TableId { get; set; }
        public string FilterExpression { get; set; }

        public Role Role { get; set; }
        public Table Table { get; set; }
    }
}