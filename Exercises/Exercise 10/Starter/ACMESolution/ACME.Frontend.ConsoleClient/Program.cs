using ACME.DataLayer.Entities;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Transactions;

namespace ACME.Frontend.ConsoleClient;

internal class Program
{
    const string databaseName = "ProductCatalog";
    const string databaseHistory = "PriceHistory";
    const string connectionString = @$"Server=.\SQLEXPRESS;Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
    const string connectionString2 = @$"Server=.\SQLEXPRESS;Database={databaseHistory};Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";
    
    static void Main()
    {
        CreateHistory();
        Exercise1(); 
    }

    private static void CreateHistory()
    {
        var builder = new DbContextOptionsBuilder<PriceHistoryContext>();
        builder.UseSqlServer(connectionString2);
        var ctx = new PriceHistoryContext(builder.Options);
        ctx.Database.EnsureDeleted();
        ctx.Database.EnsureCreated();
        Console.WriteLine("Database created");
    }

    private static void Exercise1()
    {
        // TODO 1: Close Visual Studio and open this project in Visual Studio Code
        // This is because Distributed transactions are only supported in .NET7 which
        // at the time of this writing requires a beta version of Visual Studio

        // TODO 2: Install .NET 7.0 (if not already installed. To verify run dotnet --list-sdks)
        // https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/sdk-7.0.100-rc.1-windows-x64-installer
        // Or the latest SDK version
        // https://dotnet.microsoft.com/en-us/download/dotnet/7.0

        var builder = new DbContextOptionsBuilder<PriceHistoryContext>();
        builder.UseSqlServer(connectionString2);
        var historyCtx = new PriceHistoryContext(builder.Options);

        var bld = new DbContextOptionsBuilder<ShopDatabaseContext>();
        bld.UseSqlServer(connectionString);
        var ctx = new ShopDatabaseContext(bld.Options);

        var price = new Price
        {
            BasePrice = 100,
            ShopName = "ACME",
            PriceDate = DateTime.Now,
            ProductId = 1
        };

        // TODO 3: Wrap both SaveChanges in a distributed transaction
       
        ctx.Prices.Add(price);
        historyCtx.Prices.Add(price);
        ctx.SaveChanges();
        historyCtx.SaveChanges();
        Console.WriteLine("Done");
       
        Console.WriteLine(historyCtx.Prices.Count());
        // TODO 4: Open a command prompt in this project directory and execute the following commands:
        // dotnet build
        // dotnet run
    }

}