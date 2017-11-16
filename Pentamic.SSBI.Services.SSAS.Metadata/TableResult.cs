using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentamic.SSBI.Services.SSAS.Metadata
{
    public class TableResult
    {
        public string Name { get; set; }
        public List<ColumnResult> Columns { get; set; }
        public List<MeasureResult> Measures { get; set; }
    }
}
