using Pentamic.SSBI.Models.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class Role : IAuditable, IDataModelObject
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ModelPermission ModelPermission { get; set; }

        public Model Model { get; set; }
        public List<RoleTablePermission> TablePermissions { get; set; }
        public List<UserRole> Users { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}