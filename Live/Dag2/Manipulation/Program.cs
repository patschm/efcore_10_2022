
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace Manipulation;

internal class Program
{
    public static string connectionString = @"Server=.\SQLEXPRESS;Database=ProductCatalog;Trusted_Connection=True;MultipleActiveResultSets=true;";

    static void Main(string[] args)
    {
        //ChangeTracker();
        //Insert();
        //Update();
        //Delete();
        //Conflicts();
        //Experiment1();
       Experiment2();
    }

    private static void Conflicts()
    {
        var bld = new DbContextOptionsBuilder<ProductContext>();
        bld.UseSqlServer(connectionString);
        bld.LogTo(Console.WriteLine, LogLevel.Information);
        var ctx = new ProductContext(bld.Options);

        var brand = ctx.Brands.First(b => b.Id == 63L);
        brand.Name = "Hans";

        Console.ReadLine();
        while (true)
        {
            try
            {
                ctx.SaveChanges();
                break;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var c1 = ex.Entries.First();
                Console.WriteLine(c1.CurrentValues[nameof(Brand.Name)]);
                Console.WriteLine(c1.OriginalValues[nameof(Brand.Name)]);
                var dbValues = c1.GetDatabaseValues();
                Console.WriteLine(dbValues[nameof(Brand.Name)]);
                c1.OriginalValues.SetValues(dbValues); // Client Wins
                
                //c1.OriginalValues.SetValues(dbValues); // Database Wins
                //c1.CurrentValues.SetValues(dbValues); // Database Wins
                //break;
            }
            catch (DbUpdateException e)
            {
                Console.WriteLine(e.Message);
                break;
            }
        }
    }

    private static void Experiment1()
    {
        var bld = new DbContextOptionsBuilder<ProductContext>();
        bld.UseSqlServer(connectionString);
        bld.LogTo(Console.WriteLine, LogLevel.Information);
        var ctx = new ProductContext(bld.Options);

        ctx.Brands.Where(b => b.Name.StartsWith("N")).Where(b=>b.Website.StartsWith("https")).ToList();
    }

    private static void Experiment2()
    {
        var bld = new DbContextOptionsBuilder<ProductContext>();
        bld.UseSqlServer(connectionString);
        bld.LogTo(Console.WriteLine, LogLevel.Information);
        var ctx = new ProductContext(bld.Options);

        var brand = Expression.Parameter(typeof(Brand));
        var propName = Expression.Property(brand, nameof(Brand.Name));
        var propId = Expression.Property(brand, nameof(Brand.Id));
        var expr = Expression.AndAlso(
            Expression.Equal(propName, Expression.Constant("Nikon")),
            Expression.Equal(propId, Expression.Constant(1L)));
        var lambda = Expression.Lambda(expr, brand) as Expression<Func<Brand, bool>>;
        Console.WriteLine(lambda);
        var data = ctx.Brands.Where(lambda);
        Console.WriteLine(data.Count());
    }

    private static void Delete()
    {
        var bld = new DbContextOptionsBuilder<ProductContext>();
        bld.UseSqlServer(connectionString);
        bld.LogTo(Console.WriteLine, LogLevel.Information);
        var ctx = new ProductContext(bld.Options);

        var brand = ctx.Brands.First(b => b.Id == 62L);
        brand.Id = -1;
       
        ctx = new ProductContext(bld.Options);

        var entry = ctx.Entry(brand);
        //entry.State = EntityState.Deleted;
        ShowStatus(entry);

        var brandDB = ctx.Brands.Find(62L);
        ctx.Entry(brandDB).State = EntityState.Deleted;

        Console.WriteLine(brand.Name);
        //ctx.Brands.Remove(brand);

        //entry.State = EntityState.Deleted;
        ShowStatus(entry);
        ctx.SaveChanges();
    }

    private static void Update()
    {
        var bld = new DbContextOptionsBuilder<ProductContext>();
        bld.UseSqlServer(connectionString);
        bld.LogTo(Console.WriteLine, LogLevel.Information);
        var ctx = new ProductContext(bld.Options);

        var brand = ctx.Brands.First(b => b.Id==63L);
        Console.WriteLine(brand.Name);
        brand.Name = "ACME33";

        ctx = new ProductContext(bld.Options);     
        var brandDB= ctx.Brands.Find(63L);
        //brandDB.Name = brand.Name;
       ctx.Entry(brandDB).CurrentValues.SetValues(brand);

        //var entry2= ctx.Brands.Update(brand);
        //ShowStatus(entry2);
        //var entry = ctx.Entry(brand);
        //ShowStatus(entry);
        ctx.SaveChanges();
        //ShowStatus(entry);  

    }

    private static void Insert()
    {
        var bld = new DbContextOptionsBuilder<ProductContext>();
        bld.UseSqlServer(connectionString);
        bld.LogTo(Console.WriteLine, LogLevel.Information);
        var ctx = new ProductContext(bld.Options);

        var brand = new Brand { Name = "Acme", Website = @"https:\\acmd.nl" };
        var brand2= new Brand { Name = "Acme2", Website = @"https:\\acmd.nl" };
        ctx.Brands.AddRange(brand, brand2);

        var entry = ctx.Entry(brand);
        ShowStatus(entry);

        ctx.SaveChanges();

        ShowStatus(entry);
        Console.WriteLine(brand.Id);
    }

    private static void ChangeTracker()
    {
        var bld = new DbContextOptionsBuilder<ProductContext>();
        bld.UseSqlServer(connectionString);
        var ctx = new ProductContext(bld.Options);
       
       var brand = ctx.Brands.OrderBy(p=>p.Id).Last(); 
        
        ctx.Dispose();
        ctx = new ProductContext(bld.Options);
        brand.Id = -1;
        brand.Name = "hoi";

        ctx.Brands.Attach(brand);
        var entry2 = ctx.Entry(brand);
        ShowStatus(entry2);

        //ctx.Brands.Add(new Brand { Name = "Bla" });
        //brand.Name = "Xerox";
        foreach(var entry in ctx.ChangeTracker.Entries())
        {
            ShowStatus(entry);
        }

    }

    private static void ShowStatus(EntityEntry entry)
    {
        Console.WriteLine(entry.State);
        var originals = entry.OriginalValues;
        Console.WriteLine($"Origineel: {originals.GetValue<string>(nameof(Brand.Name))}");
        var currents = entry.CurrentValues;
        Console.WriteLine($"Huidig: {currents.GetValue<string>(nameof(Brand.Name))}");
    }
}