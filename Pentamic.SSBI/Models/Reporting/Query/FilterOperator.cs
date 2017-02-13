using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting.Query
{
    public enum FilterOperator
    {
        None,
        Equals,
        NotEquals,
        In,
        Contains,
        StartsWith,
        EndsWith,
        GreaterThan,
        GreaterThanOrEqual,
        LessThan,
        LessThanOrEqual,
        Top,
        Bottom
    }
}
