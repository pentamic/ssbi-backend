using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class Relationship : IDataModelObject, IAuditable
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public int FromColumnId { get; set; }
        public int ToColumnId { get; set; }
        public DataModelObjectState State { get; set; }

        [ForeignKey("FromColumnId")]
        public Column FromColumn { get; set; }
        [ForeignKey("ToColumnId")]
        public Column ToColumn { get; set; }
        public Model Model { get; set; }
        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}
