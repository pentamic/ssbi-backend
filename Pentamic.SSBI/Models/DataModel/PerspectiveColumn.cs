using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel
{
    public class PerspectiveColumn
    {
        [Key]
        [Column(Order = 1)]
        public int PerspectiveId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ColumnId { get; set; }

        public Perspective Perspective { get; set; }
        public Column Column { get; set; }
    }
}