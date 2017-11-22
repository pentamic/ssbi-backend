namespace Pentamic.SSBI.Services.SSAS.Query
{
    public class TableQueryModel
    {
        public int ModelId { get; set; }
        public string TableName { get; set; }
        public string OrderBy { get; set; }
        public bool OrderDesc { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
}