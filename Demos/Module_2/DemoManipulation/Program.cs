
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Reflection.Metadata;

namespace DemoManipulation;

internal class Program
{
    public static string connectionString = @"Server=.\SQLEXPRESS;Database=ShopDatabase;Trusted_Connection=True;MultipleActiveResultSets=true;";

    static void Main(string[] args)
    {
        //ChangeTracker();
        //Insert();
        //Update();
        //Delete();
        //Detached();
        //Cascading();
        Concurrency();
    }

    private static void ChangeTracker()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);
        var pg = context.ProductGroups.First();

        var entry = context.Entry(pg);
        Console.WriteLine(entry.State);
        Console.WriteLine(entry.OriginalValues.GetValue<string>("Name"));
        Console.WriteLine(entry.CurrentValues.GetValue<string>("Name"));

        Console.WriteLine("============================");
        pg.Name = "Changed!";
        entry = context.Entry(pg);
        Console.WriteLine(entry.State);
        Console.WriteLine(entry.OriginalValues.GetValue<string>("Name"));
        Console.WriteLine(entry.CurrentValues.GetValue<string>("Name"));
    }

    private static void Insert()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);
        var brand = new Brand { Name = "Merk", Website = "https://brand.nl" };
        context.Brands.Add(brand);
        ShowStatus(context.Entry(brand));

        context.SaveChanges();
    }

    private static void Update()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);

        var brand = context.Brands.First(b => b.Name == "Merk");
        brand.Name = "4dotnet";
        ShowStatus(context.Entry(brand));
        context.SaveChanges();
    }

    private static void Delete()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);
        var brand = context.Brands.First(b => b.Name == "4dotnet");
        context.Remove(brand);
        ShowStatus(context.Entry(brand));
        context.SaveChanges();
    }

    private static void Detached()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);
        var brand = context.Brands.First();
        context.Dispose();

        // brand is detached
        context = new ProductContext(optionsBuilder.Options);
        ShowStatus(context.Entry(brand));

        brand.Name = "Merk";
        // Option 1) Re-attach
        context.Attach(brand);
        ShowStatus(context.Entry(brand));

        context.Dispose();
        context = new ProductContext(optionsBuilder.Options);
        // Option 2) Overwrite database values
        var dbbrand = context.Brands.First();
        context.Entry(dbbrand).CurrentValues.SetValues(brand);
        ShowStatus(context.Entry(dbbrand));
    }

    private static void Cascading()
    {
        var conStr = @"Server=.\SQLEXPRESS;Database=Cascades;Trusted_Connection=True;MultipleActiveResultSets=true;";
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(conStr);
        var context = new ProductContext(optionsBuilder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
        var pg = new ProductGroup { Name = "Group 1" };
        var brand = new Brand { Name = "Merk", Website = "https://brand.nl" };
        brand.Products.Add(new Product {Name = "Product A", Image = "image_a.jpg", Brand = brand, ProductGroup=pg });
        brand.Products.Add(new Product { Name = "Product B", Image = "image_b.jpg", Brand = brand, ProductGroup=pg});
        
        
        context.Brands.Add(brand);
        context.SaveChanges();
        Console.WriteLine("Press Enter to continue");
        Console.ReadLine();

        context = new ProductContext(optionsBuilder.Options);
        
        brand = context.Brands.First();
        context.Remove(brand);
        context.SaveChanges();
    }

    private static void Concurrency()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new ProductContext(optionsBuilder.Options);
        var brand = new Brand { Name = "Merk", Website = "https://brand.nl" };
        context.Brands.Add(brand);
        context.SaveChanges();
        Console.WriteLine("Change brand Merk in the database to something else");
        Console.WriteLine("Press Enter to continue");
        Console.ReadLine();
        brand.Name = "Random Merk";
        for (int i = 0; i < 3; i++)
        {
            try
            {
                context.SaveChanges();
                break;
            }
            catch (DbUpdateConcurrencyException duc)
            {
                Console.WriteLine(duc.Message);
                foreach (var entry in duc.Entries)
                {
                    var myValues = entry.CurrentValues;
                    var dbValues = entry.GetDatabaseValues()!;
                    Console.WriteLine($"In database: {dbValues["Name"]}. Your value: {myValues["Name"]}");
                    // Database Wins
                    entry.CurrentValues.SetValues(dbValues);
                    // Client wins
                    entry.OriginalValues.SetValues(dbValues);
                }
            }
        }
    }

    private static void ShowStatus<T>(EntityEntry<T> entry) where T: class
    {
        Console.WriteLine(entry.State);
        Console.WriteLine("Original Values");
        foreach(var prop in entry.OriginalValues.Properties)
        {
            Console.Write($"{prop.Name}: {entry.OriginalValues[prop.Name]}, ");
        }
        Console.WriteLine();
        Console.WriteLine("Current Values");
        foreach (var prop in entry.CurrentValues.Properties)
        {
            Console.Write($"{prop.Name}: {entry.CurrentValues[prop.Name]}, ");
        }
        Console.WriteLine();
    }

}