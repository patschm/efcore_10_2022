using ACME.DataLayer.Entities;
using ACME.DataLayer.Interfaces;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ACME.Datalayer.Repositories.Tests;

public class ProductRepositoryTests : BaseRepositoryTests<Product>
{
    protected override Product CreateEntity()
    {
        return new Product
        {
            Id = 100,
            Name = "Product",
            Brand = _context?.Brands.First()!,
            ProductGroup = _context?.ProductGroups.First()!,
        };
    }
    protected override IRepository<Product> CreateRepository(ShopDatabaseContext context)
    {
        return new ProductRepository(context);
    }

    protected override void ModifyEntity(Product entity)
    {
        entity.Name = "Modified";
    }
    protected override void AdditionalUpdateTest(Product entity)
    {
        Assert.Equal("Modified", entity.Name);
    }
}