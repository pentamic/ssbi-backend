using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Breeze.Persistence.EF6;
using Pentamic.SSBI.Data;

namespace Pentamic.SSBI.Services.Breeze
{
    public class DbPersistenceManager<T> : EFPersistenceManager<T>
        where T : class, new()
    {
        public DbPersistenceManager(T context) : base(context) { }
    }
}
