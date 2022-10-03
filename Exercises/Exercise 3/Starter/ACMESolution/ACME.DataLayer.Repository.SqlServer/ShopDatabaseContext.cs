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
    // TODO 2: Create DbSet properties for
    //              - ConsumerReviews
    //              - ExpertReviews
    //              - WebReviews


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
            e.HasOne(p => p.Product)
                .WithMany(p => p.Prices)
                .HasForeignKey(p => p.ProductId);
                
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
            e.HasOne(p => p.Brand)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.BrandId);
            e.HasOne(p => p.ProductGroup)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.ProductGroupId);
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
            e.Property(p => p.Timestamp)
               .IsRowVersion()
               .IsConcurrencyToken();
            e.HasOne(p => p.Reviewer)
                .WithMany(p => p.Reviews);
            e.HasOne(p => p.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(p => p.ProductId);
            // TODO 3: Remove the configurations for 
            //               - Organization
            //               - ReviewUrl
            e.Property(p => p.Organization)
              .HasMaxLength(512);
            e.Property(p => p.ReviewUrl)
                .HasMaxLength(1024);
            // TODO 4: Create a discriminator for ReviewType and map the review entities
            //               - Review on ReviewType.Generic
            //               - ConsumerReview on ReviewType.Consumer
            //               - ExpertReview on ReviewType.Expert
            //               - WebReview on ReviewType.Web

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
            e.HasOne(p => p.Product)
                .WithMany(p => p.Specifications)
                .HasForeignKey(p => p.ProductId);
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
            e.HasOne(p => p.ProductGroup)
                .WithMany(p => p.SpecificationDefinitions)
                .HasForeignKey(p => p.ProductGroupId);
        });
        // TODO 5: Configure ExpertReview properties
        //               - Organization: MaxLength=512

        // TODO 6: Configure WebReview properties
        //               - ReviewUrl: MaxLength=1024

    }
}
