using ACME.DataLayer.Repository.SqlServer;
using ACME.DataLayer.Repository.SqlServer.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
namespace ACME.Frontend.ConsoleClient;

internal class Program
{
    const string databaseName = "ProductCatalog";
    const string connectionString = @$"Server=.\SQLEXPRESS;Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=true;";

    static void Main()
    {
        Host.CreateDefaultBuilder()
           .ConfigureLogging(logBld => {
               logBld.AddFilter("Microsoft.EntityFrameworkCore.Database", LogLevel.None);
           })
           .ConfigureServices(svcs =>
           {
               svcs.AddTransient<DbCommandInterceptor, QueryInterceptor>();
               svcs.AddHostedService<ConsoleClient>();
               svcs.AddDbContext<ShopDatabaseContext>(opts =>
               {
                   opts.UseSqlServer(connectionString);
                   // TODO 2: Create a compiled model for this context and use it.
               });
           })
           .Build()
           .Start();
    }
}