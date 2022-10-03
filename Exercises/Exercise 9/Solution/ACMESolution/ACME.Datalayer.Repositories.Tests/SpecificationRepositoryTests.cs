using ACME.DataLayer.Entities;
using ACME.DataLayer.Interfaces;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ACME.Datalayer.Repositories.Tests;

public class SpecificationRepositoryTests : BaseRepositoryTests<Specification>
{
    protected override Specification CreateEntity()
    {
        return new Specification
        {
            Id = 100,
            Product = _context?.Products.First()!,
            Key = "Key"
        };
    }
    protected override IRepository<Specification> CreateRepository(ShopDatabaseContext context)
    {
        return new SpecificationRepository(context);
    }

    protected override void ModifyEntity(Specification entity)
    {
        entity.Key = "Modified";
    }
    protected override void AdditionalUpdateTest(Specification entity)
    {
        Assert.Equal("Modified", entity.Key);
    }
}