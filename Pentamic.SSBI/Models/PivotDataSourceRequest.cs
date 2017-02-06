using System.Collections.Generic;

namespace Pentamic.SSBI.Models
{
    public class PivotDataSourceRequest
    {
        public PivotDataSourceRequest()
        {
            Restrictions = new Dictionary<string, string>();
        }
        public bool Discover { get; set; }
        public string Statement { get; set; }
        public string Command { get; set; }
        public Dictionary<string, string> Restrictions { get; set; }
    }
}