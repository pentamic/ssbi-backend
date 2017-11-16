using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pentamic.SSBI.Data
{
    public class AppDbContextFactory : IDbContextFactory<AppDbContext>
    {
        public AppDbContext Create()
        {
            return new AppDbContext("Server=.;Database=SSBI_APP_DEV2;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
