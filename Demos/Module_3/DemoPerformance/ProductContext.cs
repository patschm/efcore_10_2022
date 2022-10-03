using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DemoPerformance;

internal class ProductContext : DbContext
{
    // For Compiled models
    public ProductContext(): base()
    {
    }
    public ProductContext(DbContextOptions options) : base(options)
    {
    }
    

    public DbSet<ProductGroup> ProductGroups => Set<ProductGroup>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Price> Prices => Set<Price>();
    public DbSet<Specification> Specifications => Set<Specification>();
    public DbSet<SpecificationDefinition> SpecificationDefinitions => Set<SpecificationDefinition>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Reviewer> Reviewers => Set<Reviewer>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ConsumerReview> ConsumerReviews => Set<ConsumerReview>();
    public DbSet<ExpertReview> ExpertReviews => Set<ExpertReview>();
    public DbSet<WebReview> WebReviews => Set<WebReview>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(Program.connectionString);
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Core");

        modelBuilder.Entity<Review>(conf =>
        {
            conf.HasDiscriminator(r => r.ReviewType)
                .HasValue<Review>(ReviewType.Generic)
                .HasValue<ConsumerReview>(ReviewType.Consumer)
                .HasValue<ExpertReview>(ReviewType.Expert)
                .HasValue<WebReview>(ReviewType.Web);
        });

        modelBuilder.Entity<ReviewerCredential>(conf =>
        {
            conf.ToTable("Reviewers");
        });
        modelBuilder.Entity<Reviewer>(conf =>
        {
            conf.ToTable("Reviewers")
                .HasOne(r => r.Credentials)
                .WithOne()
                .HasForeignKey<ReviewerCredential>(r => r.Id);
        });
    }
}
