using ACME.DataLayer.Entities;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;

namespace ACME.Frontend.ConsoleClient;

// TODO 7: Run the application and check the generated database (Shop3)
internal class Program
{
    const string databaseName = "Shop3";
    const string connectionString = @$"Server=.\SQLEXPRESS;Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=true;";
    
    static void Main()
    {
        var optionsBuilder = new DbContextOptionsBuilder<ShopDatabaseContext>();
        optionsBuilder.UseSqlServer(connectionString);
        var ctx = new ShopDatabaseContext(optionsBuilder.Options);

        ctx.Database.EnsureCreated();
        
        Console.WriteLine("Done!");      
    }
}