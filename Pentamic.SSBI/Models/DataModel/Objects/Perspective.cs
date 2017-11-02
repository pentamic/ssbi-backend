using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class Perspective : IDataModelObject, IAuditable
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}