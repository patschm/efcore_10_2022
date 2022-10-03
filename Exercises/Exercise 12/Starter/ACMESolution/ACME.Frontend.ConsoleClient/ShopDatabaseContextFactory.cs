using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using ACME.DataLayer.Repository.SqlServer;

namespace ACME.Frontend.ConsoleClient;

// The ef tools will use this class to generate compiled models. (IDesignTimeDbContextFactory)
// Alternatively you can use the method CreateHostBuilder(string[] args) in the Program class.
// However that will not work in this case because we use AddHostedService<ConsoleClient>()
// which is a singleton (not allowed by the tools)
public class ShopDatabaseContextFactory : IDesignTimeDbContextFactory<ShopDatabaseContext>
{
    const string databaseName = "ProductCatalog";
    const string connectionString = @$"Server=.\SQLEXPRESS;Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=true;";

    public ShopDatabaseContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ShopDatabaseContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new ShopDatabaseContext(optionsBuilder.Options);
    }
}