namespace Pentamic.SSBI.Entities
{
    public class PerspectiveColumn
    {
        public int PerspectiveId { get; set; }
        public int ColumnId { get; set; }

        public Perspective Perspective { get; set; }
        public Column Column { get; set; }
    }
}