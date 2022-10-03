using ACME.DataLayer.Entities;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ACME.Frontend.ConsoleClient;

internal class Program
{
    const string databaseName = "Shop4";
    const string connectionString = @$"Server=.\SQLEXPRESS;Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=true;";
    
    static void Main()
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(svcs => {
                // TODO 1: Register a DbContextFactory for ShopDatabaseContext
               
                svcs.AddHostedService<ConsoleHost>();
            }).Build();

        host.StartAsync().Wait();
        // TODO 6: Run the code
    }
}