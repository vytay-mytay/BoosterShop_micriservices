using Microsoft.EntityFrameworkCore;
using shop.DAL.Abstract;
using shop.Domain.Entities;
using System.Threading.Tasks;

namespace shop.DAL
{
    public class DataContext : DbContext, IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            Database.SetCommandTimeout(500);
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<OrderProduct> OrderProducts { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }
    }
}
