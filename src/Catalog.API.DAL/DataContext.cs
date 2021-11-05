using Catalog.API.DAL.Abstract;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Catalog.API.DAL
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(500);
        }

        //public virtual DbSet<UserToken> UserTokens { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

    }
}
