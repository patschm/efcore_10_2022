using DemoEntityFramework;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DemoRawQueries;

internal class Program
{
    public static string connectionString = @"Server=.\SQLEXPRESS;Database=ShopDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;";
    
    static void Main(string[] args)
    {
        BasicRaw();
        //Mixing();      
    }

    

    private static void BasicRaw()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);

        var pat = new SqlParameter("pat", "C");
        var brands = context.Brands.FromSqlRaw("SELECT * FROM Core.Brands WHERE [Name] LIKE @pat+'%'", pat);
        foreach (var brand in brands)
        {
            Console.WriteLine(brand.Name);
        }
    }
    private static void Mixing()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);

        var brands = context.Brands
            .FromSqlInterpolated($"SELECT * FROM Core.Brands")
            .OrderByDescending(b => b.Name);

        foreach (var brand in brands)
        {
            Console.WriteLine(brand.Name);
        }
    }
}