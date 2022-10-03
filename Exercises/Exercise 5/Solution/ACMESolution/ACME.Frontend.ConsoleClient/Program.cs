using ACME.DataLayer.Entities;
using ACME.DataLayer.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ACME.Frontend.ConsoleClient;

internal class Program
{
    const string databaseName = "ProductCatalog";
    const string connectionString = @$"Server=.\SQLEXPRESS;Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=true;";
    
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
        
        //var query = ctx.Brands.OrderByDescending(b => b.Name);
        var query = from b in ctx.Brands orderby b.Name select b;
        
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
        
        //var query = ctx.Brands.Where(b => b.Name!.StartsWith("B"));
        var query = from b in ctx.Brands where b.Name!.StartsWith("B") select b;

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

        //var query = ctx.Brands.Select(b=>new {b.Name, b.Website });
        var query = from b in ctx.Brands select new {b.Name, b.Website };   

        foreach(var item in query)
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

        //var query = ctx.Products
        //    .Join(ctx.Brands,
        //            p => p.BrandId,
        //            b => b.Id,
        //            (p, b) => new { BrandName = b.Name, ProductName = p.Name })
        //    .Take(20);

        var query = from p in ctx.Products
                    join b in ctx.Brands on p.BrandId equals b.Id
                    select (new { BrandName = b.Name, ProductName = p.Name });
        query = query.Take(20);

        foreach(var item in query)
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

        //var query = ctx.Prices
        //    .GroupBy(p => p.ProductId)
        //    .Select(it => new { ProductId = it.Key, Avg = it.Average(pr => pr.BasePrice) })
        //    .Join(ctx.Products,
        //        pr => pr.ProductId,
        //        p => p.Id,
        //        (pr, p) => new { PoductName = p.Name, AvgPrice = pr.Avg });

        var query = from pr in ctx.Prices
                    group pr by pr.ProductId into it
                    select new { ProductId = it.Key, Avg = it.Average(pr => pr.BasePrice) } into x
                    join p in ctx.Products on x.ProductId equals p.Id
                    select new { PoductName = p.Name, AvgPrice = x.Avg };

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

        //var query = ctx.Brands.Join(
        //        ctx.Products,
        //        b => b.Id,
        //        p => p.BrandId,
        //        (b, p) => new { BrandName = b.Name, ProductName = p.Name })
        //    .GroupBy(x => x.BrandName)
        //    .Select(it => new { BrandName = it.Key, Products = it.ToList() });

        var query = from b in ctx.Brands
                    join p in ctx.Products on b.Id equals p.BrandId
                    select new { BrandName = b.Name, ProductName = p.Name } into it
                    group it by it.BrandName into grp
                    select new { BrandName = grp.Key, Products = grp.ToList() };

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