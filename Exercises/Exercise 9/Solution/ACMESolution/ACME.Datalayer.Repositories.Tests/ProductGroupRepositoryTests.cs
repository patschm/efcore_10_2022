using ACME.DataLayer.Entities;
using ACME.DataLayer.Interfaces;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ACME.Datalayer.Repositories.Tests;

public class ProductGroupRepositoryTests : BaseRepositoryTests<ProductGroup>
{
    protected override ProductGroup CreateEntity()
    {
        return new ProductGroup
        {
            Id = 100,
            Name = "ProductGroup",
        };
    }
    protected override IRepository<ProductGroup> CreateRepository(ShopDatabaseContext context)
    {
        return new ProductGroupRepository(context);
    }

    protected override void ModifyEntity(ProductGroup entity)
    {
        entity.Name = "Modified";
    }
    protected override void AdditionalUpdateTest(ProductGroup entity)
    {
        Assert.Equal("Modified", entity.Name);
    }
}