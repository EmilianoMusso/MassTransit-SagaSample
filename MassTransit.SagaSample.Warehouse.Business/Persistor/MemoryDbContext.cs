using Microsoft.EntityFrameworkCore;

namespace MassTransit.SagaSample.Warehouse.Business.Persistor
{
    public class MemoryDbContext: DbContext
    {
        public MemoryDbContext(DbContextOptions options): base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductsFlow>().Property(x => x.ProductId).ValueGeneratedOnAdd();
            modelBuilder.Entity<ProductsFlow>().HasKey(x => x.ProductId);
        }

        public DbSet<ProductsFlow> ProductsFlow { get; set; }
    }
}
