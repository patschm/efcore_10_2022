using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Navigation;

internal class Program
{
    public static string connectionString = @"Server=.\SQLEXPRESS;Database=ProductCatalog;Trusted_Connection=True;MultipleActiveResultSets=true;";

    static void Main(string[] args)
    {
        //Simple();
        //Explicit();
        //Eager();
        //LazyL();
        Split();
    }

    private static void Split()
    {
        var bld = new DbContextOptionsBuilder<ProductContext>();
        bld.UseSqlServer(connectionString);
        bld.LogTo(Console.WriteLine, LogLevel.Information);
        var ctx = new ProductContext(bld.Options);

        var query = ctx.ProductGroups
            .Include(g => g.Products)
                .ThenInclude(p => p.Brand)
           .AsSplitQuery();

        //var q2 = ctx.Products
        //                .Include(p => p.ProductGroup)
        //                .Include(p => p.Brand)
        //                .Select(p => new { Merk = p.Brand.Name, p.Name, Group = p.ProductGroup.Name });
        //q2.ToList();

        foreach (var group in query)
        {
            Console.WriteLine($"{group.Name}");
            foreach (var product in group.Products)
            {
                Console.WriteLine($"\t-{product.Brand.Name} {product.Name}");
            }
        }
    }
    private static void LazyL()
    {
        var bld = new DbContextOptionsBuilder<ProductContext>();
        bld.UseSqlServer(connectionString);
        bld.UseLazyLoadingProxies();
        bld.LogTo(Console.WriteLine, LogLevel.Information);
        var ctx = new ProductContext(bld.Options);

        var query = ctx.ProductGroups;

        foreach (var group in query)
        {
            Console.WriteLine($"{group.Name}");
            foreach (var product in group.Products)
            {
                Console.WriteLine($"\t-{product.Brand.Name} {product.Name}");
            }
        }
    }

    private static void Eager()
    {
        var bld = new DbContextOptionsBuilder<ProductContext>();
        bld.UseSqlServer(connectionString);
        bld.LogTo(Console.WriteLine, LogLevel.Information);
        var ctx = new ProductContext(bld.Options);

        var query = ctx.ProductGroups
            .Include(g => g.Products)
                .ThenInclude(p => p.Brand);

        foreach (var group in query)
        {
            Console.WriteLine($"{group.Name}");
            foreach (var product in group.Products)
            {
                Console.WriteLine($"\t-{product.Brand.Name} {product.Name}");
            }
        }
    }

    private static void Explicit()
    {
        var bld = new DbContextOptionsBuilder<ProductContext>();
        bld.UseSqlServer(connectionString);
        bld.LogTo(Console.WriteLine, LogLevel.Information);
        var ctx = new ProductContext(bld.Options);

        var query = ctx.ProductGroups;

        foreach (var group in query)
        {
            Console.WriteLine($"{group.Name}");
            ctx.Entry(group).Collection(g => g.Products).Load();
            foreach (var product in group.Products)
            {
                ctx.Entry(product).Reference(p => p.Brand).Load();
                Console.WriteLine($"\t-{product.Brand?.Name} {product.Name}");
            }
        }
    }

    private static void Simple()
    {
        var bld = new DbContextOptionsBuilder<ProductContext>();
        bld.UseSqlServer(connectionString);
        bld.LogTo(Console.WriteLine, LogLevel.Information);
        var ctx = new ProductContext(bld.Options);

        var query = ctx.ProductGroups;

        foreach(var group in query)
        {
            Console.WriteLine($"{group.Name}");
            foreach(var product in group.Products)
            {
                Console.WriteLine($"\t-{product.Brand.Name} {product.Name}");
            }
        }
    }
}