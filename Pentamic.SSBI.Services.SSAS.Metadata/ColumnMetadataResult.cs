using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentamic.SSBI.Services.SSAS.Metadata
{
    public class ColumnMetadataResult
    {
        public string TableName { get; set; }
        public string Name { get; set; }
        public int DataType { get; set; }
        public string DisplayFolder { get; set; }
        public bool IsMeasure { get; set; }
        public bool IsHierarchy { get; set; }
    }
}
