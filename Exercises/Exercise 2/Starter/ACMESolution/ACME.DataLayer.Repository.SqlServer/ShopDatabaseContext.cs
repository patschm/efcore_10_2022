using ACME.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ACME.DataLayer.Repository.SqlServer;
public partial class ShopDatabaseContext : DbContext
{
    public ShopDatabaseContext(DbContextOptions<ShopDatabaseContext> options)
        : base(options)
    {
    }
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Price> Prices => Set<Price>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Reviewer> Reviewers => Set<Reviewer>();
    public DbSet<Specification> Specifications => Set<Specification>();
    public DbSet<SpecificationDefinition> SpecificationDefinitions => Set<SpecificationDefinition>();

   
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Core");
        
        modelBuilder.Entity<Brand>(e => {
            e.Property(p => p.Website)
                .HasMaxLength(1024);
            e.Property(p=>p.Name)
                .HasMaxLength(255)
                .IsRequired();
            e.Property(p => p.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();
        });
        modelBuilder.Entity<Price>(e => {
            e.Property(p => p.ShopName)
                .HasMaxLength(255)
                .IsRequired();
            e.Property(p => p.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            // TODO 1: Configure the 1-n navigation from Price to Product
                
        });
        modelBuilder.Entity<Product>(e => {
            e.Property(p => p.Name)
                .HasMaxLength(255)
                .IsRequired();
            e.Property(p => p.Image)
               .HasMaxLength(1024);
            e.Property(p => p.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            // TODO 2: Configure the 1-n navigation from Product to Brand

            // TODO 3: Configure the 1-n navigation from Product to ProductGroup

        });
        modelBuilder.Entity<ProductGroup>(e => {
            e.Property(p => p.Name)
                .HasMaxLength(255)
                .IsRequired();
            e.Property(p => p.Image)
               .HasMaxLength(1024);
            e.Property(p => p.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();
        });
        modelBuilder.Entity<Review>(e =>
        {
            e.Property(p => p.ReviewType)
                .HasConversion<int>();
            e.Property(p => p.Organization)
               .HasMaxLength(512);
            e.Property(p => p.ReviewUrl)
                .HasMaxLength(1024);
            e.Property(p => p.Timestamp)
               .IsRowVersion()
               .IsConcurrencyToken();
            // TODO 4: Configure the 1-n navigation from Review to Reviewer
;
            // TODO 5: Configure the 1-n navigation from Review tu Product

        });
        modelBuilder.Entity<Reviewer>(e => {
            e.Property(p => p.Name)
                .HasMaxLength(255)
                .IsRequired();
            e.Property(p => p.UserName)
               .HasMaxLength(255)
               .IsRequired();
            e.Property(p => p.Email)
               .HasMaxLength(255)
               .IsRequired();
            e.Property(p => p.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();
        });
        modelBuilder.Entity<Specification>(e => {
            e.Property(p => p.Key)
                .HasMaxLength(255)
                .IsRequired();
            e.Property(p => p.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            // TODO 6: Configure the 1-n navigation from Specification to Product

        });
        modelBuilder.Entity<SpecificationDefinition>(e => {
            e.Property(p => p.Key)
                .HasMaxLength(255)
                .IsRequired();
            e.Property(p => p.Name)
               .HasMaxLength(255)
               .IsRequired();
            e.Property(p => p.Unit)
               .HasMaxLength(127);
            e.Property(p => p.Timestamp)
                .IsRowVersion()
                .IsConcurrencyToken();
            // TODO 7: Configure the 1-n navigation from SpecificationDefinition to ProductGroup
;
        });
    }
}
