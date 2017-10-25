using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class DataModelObjectBase : IDataModelObject, IAuditable
    {
        private string _idStr { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }

        public string IdStr
        {
            get
            {
                if (_idStr == null)
                {
                    _idStr = Id.ToString();
                }
                return _idStr;
            }
        }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}