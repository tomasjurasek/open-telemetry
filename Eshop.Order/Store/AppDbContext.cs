using Eshop.Order.Saga;
using Microsoft.EntityFrameworkCore;

namespace Eshop.Order.Store
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrderState>().HasKey(s => s.CorrelationId);
        }

        public DbSet<OrderState> OrderStates { get; set; }
    }
}
