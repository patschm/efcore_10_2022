using ACME.DataLayer.Entities;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ACME.Frontend.ConsoleClient;

internal class Program
{
    const string databaseName = "ProductCatalog";
    const string connectionString = @$"Server=.\SQLEXPRESS;Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=true;";

    // TODO 0: Open Sql Server Management Studio
    // Right click on the databases node and select "Import Data-tier Application"
    // At "Import from local disk" select the file ProductCatalog.bacpac on D: or E: drive
    // Accept the defaults and continue.
    static void Main()
    {
        Exercise1();
        //Exercise2();
        //Exercise3();
        //Exercise4();
        //Exercise5();
        //Exercise6();
    }

    private static void Exercise1()
    {
        var bld = new DbContextOptionsBuilder<ShopDatabaseContext>();
        bld.UseSqlServer(connectionString);
        var ctx = new ShopDatabaseContext(bld.Options);
        // TODO 1: Create a query that reads the brands ordered by name descending.
        // Do it both with Extensions and Integrated.
        // Uncomment the method call an test the query

        var query = default(IQueryable<Brand>);
        
        foreach (var item in query)
        {
            Console.WriteLine($"[{item.Id}] {item.Name} ({item.Website})");
        }
    }
    private static void Exercise2()
    {
        var bld = new DbContextOptionsBuilder<ShopDatabaseContext>();
        bld.UseSqlServer(connectionString);
        var ctx = new ShopDatabaseContext(bld.Options);
        // TODO 2: Create a query that reads all the brands that starts with a 'B'.
        // Do it both with Extensions and Integrated.
        // Uncomment the method call an test the query

        var query = default(IQueryable<Brand>);

        foreach (var item in query)
        {
            Console.WriteLine($"[{item.Id}] {item.Name} ({item.Website})");
        }

    }
    private static void Exercise3()
    {
        var bld = new DbContextOptionsBuilder<ShopDatabaseContext>();
        bld.UseSqlServer(connectionString);
        var ctx = new ShopDatabaseContext(bld.Options);
        // TODO 3: Create a query that only reads the WebSite and Name from brands.
        // Use Projection and do it both with Extensions and Integrated.
        // Uncomment the method call an test the query

        var query = default(IQueryable<Brand>);

        foreach (var item in query)
        {
            Console.WriteLine($"{item.Name} ({item.Website})");
        }
    }
    private static void Exercise4()
    {
        var bld = new DbContextOptionsBuilder<ShopDatabaseContext>();
        bld.UseSqlServer(connectionString);
        var ctx = new ShopDatabaseContext(bld.Options);
        // TODO 4: Create a query that reads the product name and brand name.
        // Show only the top 20 items
        // Use a join and do it both with Extensions and Integrated.
        // Uncomment the method call an test the query

        var query = default(IQueryable<dynamic>);

        foreach (var item in query)
        {
            Console.WriteLine($"{item.BrandName} {item.ProductName}");
        }
    }
    private static void Exercise5()
    {
        var bld = new DbContextOptionsBuilder<ShopDatabaseContext>();
        bld.UseSqlServer(connectionString);
        var ctx = new ShopDatabaseContext(bld.Options);
        // TODO 5: Show the average product price.
        // Do it both with Extensions and Integrated.
        // Uncomment the method call an test the query

        var query = default(IQueryable<dynamic>);

        foreach (var item in query)
        {
            Console.WriteLine($"{item.PoductName}, Average Price: {item.AvgPrice}");
        }
    }
    private static void Exercise6()
    {
        var bld = new DbContextOptionsBuilder<ShopDatabaseContext>();
        bld.UseSqlServer(connectionString);
        var ctx = new ShopDatabaseContext(bld.Options);
        // TODO 6: Group Products by BrandName.
        // Do it both with Extensions and Integrated.
        // Uncomment the method call an test the query

        var query = default(IQueryable<dynamic>);

        foreach (var item in query)
        {
            Console.WriteLine($"{item.BrandName}");
            foreach (var p in item.Products)
            {
                Console.WriteLine($"\t{p.ProductName}");
            }
        }
    }
}