using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel
{
    public enum ColumnType
    {
        Data = 1,
        Calculated = 2,
        RowNumber = 3,
        CalculatedTableColumn = 4
    }
}