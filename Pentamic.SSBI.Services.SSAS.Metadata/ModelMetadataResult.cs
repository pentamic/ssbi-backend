using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentamic.SSBI.Services.SSAS.Metadata
{
    public class ModelMetadataResult
    {
        public string Name { get; set; }
        public List<TableMetadataResult> Tables { get; set; }
    }
}
