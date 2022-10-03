using ACME.DataLayer.Entities;
using ACME.DataLayer.Interfaces;
using ACME.DataLayer.Repository.SqlServer;
using ACME.DataLayer.Repository.SqlServer.Interceptors;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace ACME.Datalayer.Repositories.Tests;

public abstract class BaseRepositoryTests<T> where T: Entity, new()
{
    protected ShopDatabaseContext? _context;
    protected async Task<ShopDatabaseContext> CreateContextAsync()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<ShopDatabaseContext>();
        builder.UseSqlite(connection);
        _context = new ShopDatabaseContext(builder.Options, new QueryInterceptor(NullLogger<QueryInterceptor>.Instance));
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();
        await SeedAsync(_context);
        return _context;
    }
    private async Task SeedAsync(ShopDatabaseContext context)
    {
        var rnd = new Random();
        for (int i = 1; i <= 10; i++)
        {
            var brand = new Brand
            {
                Id = i,
                Name = $"Brand {i}",
                Website = $"https://brand_{i}"
            };
            context.Brands.Add(brand);
            var group = new ProductGroup
            {
                Id = i,
                Name = $"ProductGroup {i}",
                Image = $"productgroup_image_{i}.jpg"
            };
            context.ProductGroups.Add(group);
            var product = new Product
            {
                Id = i,
                Name = "Product",
                Brand = brand,
                ProductGroup = group,
                Image = "product.jpg"
            };
            context.Products.Add(product);
            var reviewer = new Reviewer
            {
                Id = i,
                Email = $"tester{i}@test.nl",
                Name = $"Tester {i}",
                UserName = $"tester{i}",
            };
            context.Reviewers.Add(reviewer);
            var creview = new ConsumerReview
            {
                Id = i,
                DateBought = DateTime.Now.AddMonths(-(int)i),
                Reviewer = reviewer,
                Score = (byte)(i % 5),
                Text = $"Text {i}",
                Product = product
            };
            context.ConsumerReviews.Add(creview);
            var ereview = new ExpertReview
            {
                Id = i+10,
                Organization = $"Company {i+10}",
                Reviewer = reviewer,
                Score = (byte)(i % 5),
                Text = $"Text {i+10}",
                Product = product
            };
            context.ExpertReviews.Add(ereview);
            var wreview = new WebReview
            {
                Id = i + 20,
                ReviewUrl = $"https://test{i + 20}.nl/review{i + 20}",
                Reviewer = reviewer,
                Score = (byte)(i % 5),
                Text = $"Text {i + 20}",
                Product = product
            };
            context.WebReviews.Add(wreview);
            var price = new Price
            {
                Id = i,
                BasePrice = rnd.Next(10, 1000),
                ShopName = $"Shop {i}",
                Product = product,
                PriceDate = DateTime.Now.AddDays(-i)
            };
            context.Prices.Add(price);
            var specdef = new SpecificationDefinition
            {
                Id = i,
                Description = $"Description {i}",
                Key = $"key_{i}",
                Name = $"Name {i}",
                ProductGroup = group,
                Type = $"type{rnd.Next(1, 4)}",
                Unit = $"Unit{i}"
            };
            context.SpecificationDefinitions.Add(specdef);
            var spec = new Specification
            {
                Id = i,
                Key = $"key_{i}",
                Product = product,
                BoolValue = specdef.Type == "type1" ? rnd.Next(0, 2) == 1 : null,
                NumberValue = specdef.Type == "type2" ? i : null,
                StringValue = specdef.Type == "type3" ? $"value {i}" : null
            };
            context.Specifications.Add(spec);

            await context.SaveChangesAsync();
        }
        
        await context.SaveChangesAsync();
    }
    protected abstract IRepository<T> CreateRepository(ShopDatabaseContext context);
    protected abstract T CreateEntity();
    protected abstract void ModifyEntity(T entity);
    protected virtual void AdditionalUpdateTest(T entity)
    {
    }
    protected virtual void AdditionalInsertTest(T entity)
    {
    }

    [Theory]
    [InlineData(5)]
    [InlineData(6)]
    [InlineData(7)]
    public async Task Test_FindAync(long id)
    {
        var context = await CreateContextAsync();
        var repo = CreateRepository(context);

        var result = await repo.FindAsync(r => r.Id == id);
        Assert.NotNull(result);
        Assert.True(result.Count() > 0);
        Assert.Collection(result, r => Assert.Equal(id, r.Id));
    }
    [Fact]
    public async Task Test_GetAllPagedAync()
    {
        var context = await CreateContextAsync();
        var repo = CreateRepository(context);

        var result = await repo.GetAllAsync(2, 3);
        Assert.NotNull(result);
        Assert.True(result.Count() == 3);
        Assert.True(result.First().Id == 4);
    }
    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    public async Task Test_GetByIdAync(long id)
    {
        var context = await CreateContextAsync();
        var repo = CreateRepository(context);

        var result = await repo.GetByIdAsync(id);
        Assert.NotNull(result);
        Assert.True(result.Id == id);
    }
    [Fact]
    public async Task Test_InsertAync()
    {
        var context = await CreateContextAsync();
        var repo = CreateRepository(context);

        var entity = CreateEntity();
        await repo.InsertAsync(entity);
        var result = await repo.SaveAsync();
        Assert.True(result == 1);
        var inserted = await repo.GetByIdAsync(entity.Id);
        Assert.NotNull(inserted);
        AdditionalInsertTest(entity);
    }
    [Fact]
    public async Task Test_UpdateAync()
    {
        var context = await CreateContextAsync();
        var repo = CreateRepository(context);

        var entity = await repo.GetByIdAsync(1);
        ModifyEntity(entity);
        await repo.UpdateAsync(entity);
        var result = await repo.SaveAsync();
        Assert.True(result == 1);
        var changed = await repo.GetByIdAsync(1);
        Assert.NotNull(changed);
        AdditionalUpdateTest(changed);
    }
    [Fact]
    public async Task Test_DeleteAync()
    {
        var context = await CreateContextAsync();
        var repo = CreateRepository(context);

        var entity = await repo.GetByIdAsync(1);
        await repo.DeleteAsync(entity);
        var result = await repo.SaveAsync();
        Assert.True(result >= 1);
        await Assert.ThrowsAnyAsync<Exception>(() => repo.GetByIdAsync(1));
    }
}
