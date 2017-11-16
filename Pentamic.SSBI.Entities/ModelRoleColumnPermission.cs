namespace Pentamic.SSBI.Entities
{
    public class ModelRoleColumnPermission
    {
        public int RoleId { get; set; }
        public int ColumnId { get; set; }
        public bool MetadataPermission { get; set; }

        public ModelRole Role { get; set; }
        public Table Column { get; set; }
    }
}