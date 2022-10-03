using ACME.DataLayer.Entities;
using ACME.DataLayer.Interfaces;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ACME.Datalayer.Repositories.Tests;

public class BrandRepositoryTests : BaseRepositoryTests<Brand>
{
    protected override Brand CreateEntity()
    {
        return new Brand
        {
            Id = 100,
            Name = "Brand",
            Website = "https://brand.nl"
        };
    }
    protected override IRepository<Brand> CreateRepository(ShopDatabaseContext context)
    {
        return new BrandRepository(context);
    }

    protected override void ModifyEntity(Brand entity)
    {
        entity.Name = "Modified";
    }
    protected override void AdditionalUpdateTest(Brand entity)
    {
        Assert.Equal("Modified", entity.Name);
    }
}