using System.Data.Entity.Infrastructure;

namespace Pentamic.SSBI.Data
{
    public class AppDbContextFactory : IDbContextFactory<AppDbContext>
    {
        public AppDbContext Create()
        {
            return new AppDbContext("NULL");
        }
    }
}
