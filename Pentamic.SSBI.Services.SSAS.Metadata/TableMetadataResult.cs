using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentamic.SSBI.Services.SSAS.Metadata
{
    public class TableMetadataResult
    {
        public string Name { get; set; }
        public List<ColumnMetadataResult> Fields { get; set; }
        //public List<MeasureMetadataResult> Measures { get; set; }
    }
}
