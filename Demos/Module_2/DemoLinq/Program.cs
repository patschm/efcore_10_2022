using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Logging;

namespace DemoLinq;

internal class Program
{
    public static string connectionString = @"Server=.\SQLEXPRESS;Database=ProductCatalog;Trusted_Connection=True;MultipleActiveResultSets=true;";

    static void Main(string[] args)
    {
        //ViaExtensions();
        ViaIntegrated();
    }

    private static void ViaExtensions()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        optionsBuilder.LogTo(s => Console.WriteLine(s), LogLevel.Information);
        var context = new ProductContext(optionsBuilder.Options);

        var subset = new long[] { 1, 2, 3, 4 };
        var query = context.Products.Where(p => subset.Contains(p.Id));
        query.ToList();
        var q2 = context.Products.Select(p => new { p.Name, p.Image });
        q2.ToList();
        //var all = context.Brands
        //                .Where(b => b.Name!.StartsWith("C"))
        //                .Join(context.Products, b => b.Id, p => p.BrandId, (b, p) => new { b, p })
        //                .Select(cmp => new {Brand = cmp.b.Name, Product=cmp.p});

        //foreach(var item in all)
        //{
        //    Console.WriteLine($"{item.Brand} {item.Product.Name}");         
        //}

        // GroupJoin not supported yet :(
        //var gj = context.Brands
        //                .Where(b => b.Name!.StartsWith("C"))
        //                .GroupJoin(context.Products, b => b.Id, p => p.BrandId, (b, p) => new { Brand = b, Products = p });
        //foreach(var item in gj)
        //{
        //    Console.WriteLine($"{item.Brand.Name}");
        //    foreach(var p in item.Products)
        //    {
        //        Console.WriteLine($"\t{p.Name}");
        //    }
        //}
    }

    private static void ViaIntegrated()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        optionsBuilder.LogTo(s => Console.WriteLine(s), LogLevel.Information);
        var context = new ProductContext(optionsBuilder.Options);

        var all = from item in context.Brands
                  where item.Name!.StartsWith("C")
                  join item2 in context.Products on item.Id equals item2.BrandId
                  select new { Brand = item.Name, Product = item2 };

        foreach (var item in all)
        {
            //Console.WriteLine($"{item.Brand} {item.Product.Name}");
        }

        var q2 = from p in context.Products group p by p.BrandId into it select new { Key=it.Key, Data = it.ToList() };
        q2.ToList();
    }
}