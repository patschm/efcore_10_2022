using Microsoft.EntityFrameworkCore;

namespace DemoInheritance;

internal class Program
{
    public static string connectionString = @"Server=.\SQLEXPRESS;Database=InheritDb;Trusted_Connection=True;MultipleActiveResultSets=true;";

    static void Main(string[] args)
    {
        DemoInheritance();
        
    }

    private static void DemoInheritance()
    {
        var optionsBuilder = new DbContextOptionsBuilder();
        optionsBuilder.UseSqlServer(connectionString);
        var context = new MyContext(optionsBuilder.Options);
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

   
        //foreach (var review in context.WebReviews)
        foreach (var review in context.Reviews.OfType<WebReview>())
        {
            Console.WriteLine(review.ReviewUrl);
        }
    }
}