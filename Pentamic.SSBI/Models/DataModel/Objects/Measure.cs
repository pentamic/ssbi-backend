using System;

namespace Pentamic.SSBI.Models.DataModel.Objects
{
    public class Measure : DataModelObjectBase
    {
        public int ModelId { get; set; }
        public int TableId { get; set; }
        public string Description { get; set; }
        public string DisplayFolder { get; set; }
        public string Expression { get; set; }
        public string FormatString { get; set; }

        public Table Table { get; set; }
    }
}
