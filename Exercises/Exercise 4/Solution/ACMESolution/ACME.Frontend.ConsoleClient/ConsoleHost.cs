using ACME.DataLayer.Entities;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace ACME.Frontend.ConsoleClient;

internal class ConsoleHost : IHostedService
{
    // TODO 2: Define a private field _factory of type IDbContextFactory<ShopDatabaseContext>
    // and initialize it in a constructor (through dependency injection)
    private readonly IDbContextFactory<ShopDatabaseContext> _factory;

    public ConsoleHost(IDbContextFactory<ShopDatabaseContext> factory)
    {
        _factory = factory;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        CreateDatabase();
        WriteData();
        ReadData();
        return Task.CompletedTask;
    }

    private void CreateDatabase()
    {
        Console.WriteLine("Creating Database");
        // TODO 3: Create a context (ctx) from the injected factory
        // Uncomment the code below
        var ctx = _factory.CreateDbContext();
        ctx.Database.EnsureCreated();
        Console.WriteLine("Database created");
    }
    private void WriteData()
    {
        Console.WriteLine("Writing some data");
        // TODO 4: Create a context (ctx) from the injected factory
        // Uncomment the code below
        var ctx = _factory.CreateDbContext();
        ctx.Brands.Add(new Brand { Name = "Test", Website = "https://www.test.nl" });
        ctx.SaveChanges();
        Console.WriteLine("Written some data");
    }
    private void ReadData()
    {
        Console.WriteLine("Reading data");
        // TODO 5: Create a context (ctx) from the injected factory
        // Uncomment the code below
        var ctx = _factory.CreateDbContext();
        foreach(var brand in ctx.Brands)
        {
            Console.WriteLine($"{brand.Name} ({brand.Website})");
        }
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}