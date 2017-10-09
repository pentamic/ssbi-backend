using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AnalysisServices;
using Pentamic.SSBI.Models.DataModel.Connections;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class ModelConnection
    {
        [Key]
        [Column(Order = 1)]
        public int ModelId { get; set; }
        [Key]
        [Column(Order = 2)]
        public int ConnectionId { get; set; }

        public Model Model { get; set; }
        public Connection Connection { get; set; }
    }
}