using System.Collections.Generic;

namespace Pentamic.SSBI.Services.SSAS.Query
{
    public class QueryModel
    {
        public int ModelId { get; set; }
        public List<string> Columns { get; set; }
        public List<string> Filters1 { get; set; }
        public List<string> Filters2 { get; set; }
        public List<string> Values { get; set; }
        public List<string> OrderBy { get; set; }

        public QueryModel()
        {
            Columns = new List<string>();
            Filters1 = new List<string>();
            Filters2 = new List<string>();
            Values = new List<string>();
            OrderBy = new List<string>();
        }
    }
}