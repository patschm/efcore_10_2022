using ACME.DataLayer.Entities;
using ACME.DataLayer.Interfaces;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ACME.Datalayer.Repositories.Tests;

public class ReviewRepositoryTests : BaseRepositoryTests<Review>
{
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

    protected override Review CreateEntity()
    {
        return new ConsumerReview
        {
            Id = 100,
            DateBought = DateTime.Now.AddDays(-10),
            Product = _context?.Products.First(),
            Reviewer = _context?.Reviewers.First(),
            Score = 3,
            Text = "Testing..."
        };
    }
    protected override IRepository<Review> CreateRepository(ShopDatabaseContext context)
    {
        return new ReviewRepository(context);
    }

    protected override void ModifyEntity(Review entity)
    {
        entity.Text = "Modified";
    }
    protected override void AdditionalUpdateTest(Review entity)
    {
        Assert.Equal("Modified", entity.Text);
    }
}