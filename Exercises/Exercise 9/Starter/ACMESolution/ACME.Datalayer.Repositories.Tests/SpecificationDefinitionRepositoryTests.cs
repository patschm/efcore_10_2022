using ACME.DataLayer.Entities;
using ACME.DataLayer.Interfaces;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ACME.Datalayer.Repositories.Tests;

public class SpecificationDefinitionRepositoryTests : BaseRepositoryTests<SpecificationDefinition>
{
    protected override SpecificationDefinition CreateEntity()
    {
        return new SpecificationDefinition
        {
            Id = 100,
            Name = "SpecDef",
            ProductGroup = _context?.ProductGroups.First()!,
            Key = "Key"
        };
    }
    protected override IRepository<SpecificationDefinition> CreateRepository(ShopDatabaseContext context)
    {
        return new SpecificationDefinitionRepository(context);
    }

    protected override void ModifyEntity(SpecificationDefinition entity)
    {
        entity.Name = "Modified";
    }
    protected override void AdditionalUpdateTest(SpecificationDefinition entity)
    {
        Assert.Equal("Modified", entity.Name);
    }
}