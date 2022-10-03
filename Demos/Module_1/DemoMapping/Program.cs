using Microsoft.EntityFrameworkCore;

namespace DemoMapping;

internal class Program
{
    public static string connectionString = @"Server=.\SQLEXPRESS;Database=ShopDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;";

    static void Main(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);
        foreach (Merk brand in context.Merken.Include(b => b.Produkten))
        {
            Console.WriteLine($"{brand.Naam}");
            foreach (Produkt product in brand.Produkten.Take(5))
            {
                Console.WriteLine($"\t{product.Naam}");
            }
        }
    }
}