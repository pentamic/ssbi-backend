using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pentamic.SSBI.Models
{
    public class AuthGroup
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<UserGroup> Users { get; set; }
    }
}