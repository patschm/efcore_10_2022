using ACME.DataLayer.Entities;
using ACME.DataLayer.Interfaces;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ACME.Datalayer.Repositories.Tests;

public class PriceRepositoryTests : BaseRepositoryTests<Price>
{
    protected override Price CreateEntity()
    {
        return new Price
        {
            Id = 10000,
            BasePrice = 1000,
            Product =_context?.Products.First()!,
            ShopName = "Shop"
        };
    }
    protected override IRepository<Price> CreateRepository(ShopDatabaseContext context)
    {
        return new PriceRepository(context);
    }

    protected override void ModifyEntity(Price entity)
    {
        entity.BasePrice = 1000000;
    }
    protected override void AdditionalUpdateTest(Price entity)
    {
        Assert.Equal(1000000, entity.BasePrice);
    }
}