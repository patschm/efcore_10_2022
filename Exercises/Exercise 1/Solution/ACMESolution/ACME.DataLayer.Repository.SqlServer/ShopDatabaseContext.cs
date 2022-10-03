using ACME.DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ACME.DataLayer.Repository.SqlServer;


// TODO 1: Create the ShopDatabaseContext
// TODO 1a: Create a constructor that accepts an argument of type DbContextOptions
// TODO 1b: Create DbSet Properties for
//                - Brands
//                - Prices
//                - Products
//                - ProductGroups
//                - Reviews
//                - Reviewers
//                - Specifications
//                - SpecificationDefinitions
// TODO 2: Override the OnModelCreating
// TODO 2a: In the OnModelCreated set the default database schema (dbo) to Core
// TODO 2b: Configure Brand Properties
//                 - Website: MaxLength=1024;
//                 - Name: MaxLength=255, Required
//                 - Timestamp: RowVersion, ConcurrencyToken
// TODO 2c: Configure Price Properties
//                - ShopName: MaxLength=255, Required
//                 - Timestamp: RowVersion, ConcurrencyToken
// TODO 2d: Configure Product Properties
//                 - Image: MaxLength=1024;
//                 - Name: MaxLength=255, Required
//                 - Timestamp: RowVersion, ConcurrencyToken
// TODO 2d: Configure ProductGroup Properties
//                 - Image: MaxLength=1024;
//                 - Name: MaxLength=255, Required
//                 - Timestamp: RowVersion, ConcurrencyToken
// TODO 2e: Configure Reviewer Properties
//                 - Name: MaxLength=255, Required
//                 - UserName: MaxLength=255, Required
//                 - Email: MaxLength=255, Required
//                 - Timestamp: RowVersion, ConcurrencyToken
// TODO 2f: Configure Review Properties
//                 - ReviewType: Convert to int
//                 - Organization: MaxLength=512
//                 - ReviewUrl: MaxLength=1024
//                 - Timestamp: RowVersion, ConcurrencyToken
// TODO 2g: Configure Specification Properties
//                 - Key: MaxLength=255, Required
//                 - Timestamp: RowVersion, ConcurrencyToken
// TODO 2h: Configure SpecificationDefinition Properties
//                 - Key: MaxLength=255, Required
//                 - Name: MaxLength=255, Required
//                 - Unit: MaxLength=127, Required
//                 - Timestamp: RowVersion, ConcurrencyToken
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
        });
    }
}
