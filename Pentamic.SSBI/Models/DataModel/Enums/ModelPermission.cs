using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel.Enums
{
    public enum ModelPermission
    {
        None = 1,
        Read = 2,
        ReadRefresh = 3,
        Refresh = 4,
        Administrator = 5
    }
}