namespace Pentamic.SSBI.Entities
{
    public class ModelRoleTablePermission
    {
        public int RoleId { get; set; }
        public int TableId { get; set; }
        public string FilterExpression { get; set; }
        public bool MetadataPermission { get; set; }

        public ModelRole Role { get; set; }
        public Table Table { get; set; }
    }
}