using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class RoleColumnPermission
    {
        [Key]
        [Column(Order = 1)]
        public int RoleId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ColumnId { get; set; }
        public bool MetadataPermission { get; set; }

        public Role Role { get; set; }
        public Table Column { get; set; }
    }
}