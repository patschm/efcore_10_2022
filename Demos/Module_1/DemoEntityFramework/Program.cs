using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DemoEntityFramework;

internal class Program
{
    public static string connectionString = @"Server=.\SQLEXPRESS;Database=ShopDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;";
    
    static void Main(string[] args)
    {
        ReadData();
       // InsertData();
       // UpdateData();
       // DeleteData();
    }

    private static void ReadData()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);
        foreach (Brand brand in context.Brands.Include(b=>b.Products))
        {
            Console.WriteLine($"{brand.Name}");
            foreach (Product product in brand.Products.Take(5))
            {
                Console.WriteLine($"\t{product.Name}");
            }
        }
    }

    private static void InsertData()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);
        var p1 = new Product { Name = "Test", Image = "test.jpg", BrandId = 1 };
        context.Products.Add(p1);
        context.SaveChanges();
    }

    private static void UpdateData()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);
        var p1 = context.Products.FirstOrDefault(p=>p.Image == "test.jpg");
        p1!.Name = "Testig";
        context.SaveChanges();
    }

    private static void DeleteData()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);
        var p1 = context.Products.FirstOrDefault(p => p.Image == "test.jpg");
        context.Remove(p1!);
        context.SaveChanges();
    }
}