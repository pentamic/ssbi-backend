﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.Reporting
{
    public class ReportSharing
    {
        public string UserId { get; set; }
        public int ReportId { get; set; }
        public string Permission { get; set; }
    }
}