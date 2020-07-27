using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class Hierarchy : IDataModelObject, IAuditable
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayFolder { get; set; }
        public List<Level> Levels { get; set; }
        public Table Table { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}