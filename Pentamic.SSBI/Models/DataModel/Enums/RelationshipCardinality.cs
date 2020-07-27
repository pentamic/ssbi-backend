using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models.DataModel
{
    public enum RelationshipCardinality
    {
        ManyToOne = 1,
        OneToOne = 2,
        OneToMany = 3
    }
}