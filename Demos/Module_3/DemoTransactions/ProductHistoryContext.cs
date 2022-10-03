using Microsoft.EntityFrameworkCore;

namespace DemoTransactions;

internal class ProductHistoryContext : DbContext
{
    public ProductHistoryContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Brand> Brands => Set<Brand>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Core");

        
    }
}
