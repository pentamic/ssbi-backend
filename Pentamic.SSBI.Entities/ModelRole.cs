using System;
using System.Collections.Generic;

namespace Pentamic.SSBI.Entities
{
    public class ModelRole : IAuditable, IDataModelObject
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ModelPermission ModelPermission { get; set; }

        public Model Model { get; set; }
        public List<ModelRoleTablePermission> TablePermissions { get; set; }
        public List<UserModelRole> Users { get; set; }

        public string CreatedBy { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public string ModifiedBy { get; set; }
        public DateTimeOffset ModifiedAt { get; set; }
    }
}