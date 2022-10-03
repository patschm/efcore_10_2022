using Microsoft.EntityFrameworkCore;

namespace DemoInheritance;

internal class MyContext : DbContext
{
    public MyContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<ConsumerReview> ConsumerReviews => Set<ConsumerReview>();
    public DbSet<ExpertReview> ExpertReviews => Set<ExpertReview>();
    public DbSet<WebReview> WebReviews => Set<WebReview>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Core");

        // Table-per-hierarchy TPH (Discriminator Field)
        modelBuilder.Entity<Review>(conf =>
        {
            conf.HasDiscriminator(r => r.ReviewType)
                .HasValue<Review>(ReviewType.Generic)
                .HasValue<ConsumerReview>(ReviewType.Consumer)
                .HasValue<ExpertReview>(ReviewType.Expert)
                .HasValue<WebReview>(ReviewType.Web);
        });

        // Table-per-Type TPT (Derived classes have their own table)
        // Is slower than TPH
        // modelBuilder.Entity<Review>().ToTable("Reviews");
        // modelBuilder.Entity<ConsumerReview>().ToTable("ConsumerReviews");
        // modelBuilder.Entity<ExpertReview>().ToTable("ExpertReviews");
        // modelBuilder.Entity<WebReview>().ToTable("WebReviews");

        // Entity Splitting. Split large tables into multiple entities
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
       
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seeds
        modelBuilder.Entity<ReviewerCredential>().HasData(
            new ReviewerCredential
            {
                Id = 1,
                PasswordHash = "SHA1",
                PasswordSalt = "Salty",
                UserName = "Hans"
            },
            new ReviewerCredential
            {
                Id = 2,
                PasswordHash = "SHA1",
                PasswordSalt = "Salty",
                UserName = "Mary"
            },
            new ReviewerCredential
            {
                Id = 3,
                PasswordHash = "SHA1",
                PasswordSalt = "Salty",
                UserName = "Justin"
            });
        modelBuilder.Entity<Reviewer>().HasData(
            new Reviewer
            {
                Id = 1,
                Email = "hans@blah.nl",
                Name = "Hans"
            },
            new Reviewer
            {
                Id = 2,
                Email = "mary@blah.nl",
                Name = "Mary"
            },
            new Reviewer
            {
                Id = 3,
                Email = "justin@blah.nl",
                Name = "Justin"
            });
        modelBuilder.Entity<WebReview>().HasData(new WebReview
        {
            Id = 1,
            ReviewerId = 1,
            ReviewUrl = "https://Somewhere.nl",
            Score = 4,
            Text = "Good stuff"
        });
        modelBuilder.Entity<ConsumerReview>().HasData(new ConsumerReview
        {
            Id = 2,
            ReviewerId = 2,
            DateBought = DateTime.Now.AddMonths(-3),
            Score = 5,
            Text = "Excellent"
        });
        modelBuilder.Entity<ExpertReview>().HasData(new ExpertReview
        {
            Id = 3,
            ReviewerId = 3,
            Organization = "ACME",
            Score = 3,
            Text = "Meh"
        });
    }
}
