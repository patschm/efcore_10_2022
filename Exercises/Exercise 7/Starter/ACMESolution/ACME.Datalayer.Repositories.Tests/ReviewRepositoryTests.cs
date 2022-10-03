using ACME.DataLayer.Entities;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ACME.Datalayer.Repositories.Tests;

public class ReviewRepositoryTests
{
    private async Task<ShopDatabaseContext> CreateContextAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ShopDatabaseContext>();
        builder.UseSqlite(connection);
        var context = new ShopDatabaseContext(builder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        await SeedAsync(context);
        return context;
    }
    private async Task SeedAsync(ShopDatabaseContext context)
    {
        var productGroup = new ProductGroup
        {
            Id = 1,
            Name = "Group",
            Image = "Image"
        };
        var brand = new Brand { Id = 1, Name = "Brand", Website = "https://brand.nl" };
        var product = new Product { Id = 1, Name = "Product", Brand = brand, ProductGroup = productGroup, Image = "product.jpg" };
        await context.Products.AddAsync(product);

        var reviewers = new List<Reviewer>();
        for(long i=1; i <= 10; i++)
        {
            reviewers.Add(new Reviewer
            {
                Id = i,
                Email = $"test{i}@test.nl",
                Name = $"Test {i}",
                UserName=$"test{i}",
            }) ;
        }
        await context.Reviewers.AddRangeAsync(reviewers);
        await context.SaveChangesAsync();
        for (long i = 1; i <= 10; i++)
        {
            context.ConsumerReviews.Add(new ConsumerReview
            {
                Id = i,
                DateBought = DateTime.Now.AddMonths(-(int)i),
                Reviewer = reviewers[(int)i - 1],
                Score = (byte)(i % 5),
                Text = $"Text {i}",
                Product = product
            });
        }
        for (long i = 11; i <= 20; i++)
        {
            context.ExpertReviews.Add(new ExpertReview
            {
                Id = i,
                Organization = $"Company {i}",
                Reviewer = reviewers[(int)i - 11],
                Score = (byte)(i % 5),
                Text = $"Text {i}",
                Product=product
            });
        }
        for (long i = 21; i <= 30; i++)
        {
            context.WebReviews.Add(new WebReview
            {
                Id = i,
                ReviewUrl = $"https://test{i}.nl/review{i}",
                Reviewer = reviewers[(int)i - 21],
                Score = (byte)(i % 5),
                Text = $"Text {i}",
                Product = product
            });
        }
        await context.SaveChangesAsync();
    }

    [Theory]
    [InlineData(5)]
    [InlineData(15)]
    [InlineData(25)]
    public async Task Test_FindAync(long id)
    {
        var context = await CreateContextAsync();
        var repo = new ReviewRepository(context);

        var result = await repo.FindAsync(r => r.Id == id);
        Assert.NotNull(result);
        Assert.True(result.Count() > 0);
        Assert.Collection(result, r => Assert.Equal(id, r.Id));
    }
    [Theory]
    [InlineData(5, typeof(ConsumerReview))]
    [InlineData(15, typeof(ExpertReview))]
    [InlineData(25, typeof(WebReview))]
    public async Task Test_ReviewTypeAync(long id, Type type)
    {
        var context = await CreateContextAsync();
        var repo = new ReviewRepository(context);

        var result = await repo.FindAsync(r => r.Id == id);
        Assert.NotNull(result);
        Assert.True(result.Count() > 0);
        Assert.Collection(result, r => Assert.IsType(type, r));
    }
    [Fact]
    public async Task Test_GetAllPagedAync()
    {
        var context = await CreateContextAsync();
        var repo = new ReviewRepository(context);

        var result = await repo.GetAllAsync(2, 5);
        Assert.NotNull(result);
        Assert.True(result.Count() == 5);
        Assert.True(result.First().Id == 6);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Test_GetByIdAync(long id)
    {
        var context = await CreateContextAsync();
        var repo = new ReviewRepository(context);

        var result = await repo.GetByIdAsync(id);
        Assert.NotNull(result);
        Assert.True(result.Id == id);
    }
    [Fact]
    public async Task Test_InsertAync()
    {
        var context = await CreateContextAsync();
        var repo = new ReviewRepository(context);
        
        var review = new ExpertReview {
            Id = 100,
            Product = context.Products.First(),
            Reviewer = context.Reviewers.First(),
            Organization = "Experts",
            Score = 5,
            Text="Review"
        };
        await repo.InsertAsync(review);
        var result = await repo.SaveAsync();
        Assert.True(result == 1);
        var inserted = await repo.GetByIdAsync(100);
        Assert.NotNull(inserted);
    }
    [Fact]
    public async Task Test_UpdateAync()
    {
        var context = await CreateContextAsync();
        var repo = new ReviewRepository(context);

        var review = await repo.GetByIdAsync(1);
        review.Text = "Modified";
        await repo.UpdateAsync(review);
        var result = await repo.SaveAsync();
        Assert.True(result == 1);
        var inserted = await repo.GetByIdAsync(1);
        Assert.NotNull(inserted);
        Assert.True(inserted.Text == review.Text);
    }
    [Fact]
    public async Task Test_DeleteAync()
    {
        var context = await CreateContextAsync();
        var repo = new ReviewRepository(context);

        var review = await repo.GetByIdAsync(1);
        await repo.DeleteAsync(review);
        var result = await repo.SaveAsync();
        Assert.True(result == 1);
        await Assert.ThrowsAnyAsync<Exception>(() => repo.GetByIdAsync(1));
    }

}