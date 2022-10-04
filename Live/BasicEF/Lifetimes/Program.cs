using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Scaffold;

internal class Program
{
    public const string conStr = @"Server=.\sqlexpress;Database=ProductCatalog;Trusted_Connection=True;MultipleActiveResultSets=true;";

    static void Main(string[] args)
    {
        //LeanAndMean();
        //InControl();
        //ViaDI();
        //ViaDIPool();
        ViaDIFactory();
    }

    private static void ViaDIFactory()
    {
        Host.CreateDefaultBuilder()
           .ConfigureServices(scvs => {
               scvs.AddDbContextFactory<ProductCatalogContext>(opts => {
                   opts.UseSqlServer(conStr);
               });
               scvs.AddHostedService<DIFactoryApp>();
           })
           .Build()
           .Start();
    }

    private static void ViaDI()
    {
        Host.CreateDefaultBuilder()
            .ConfigureServices(scvs => {
                scvs.AddDbContext<ProductCatalogContext>(opts => { 
                    opts.UseSqlServer(conStr);
                });
                scvs.AddHostedService<DIFactoryApp>();
            })
            .Build()
            .Start();          
    }

    private static void ViaDIPool()
    {
        Host.CreateDefaultBuilder()
            .ConfigureServices(scvs => {
                scvs.AddDbContextPool<ProductCatalogContext>(opts => {
                    opts.UseSqlServer(conStr);
                });
                scvs.AddHostedService<DIFactoryApp>();
            })
            .Build()
            .Start();
    }

    private static void InControl()
    {
        var connection = new SqlConnection(conStr);
        connection.Open();
        var buider = new DbContextOptionsBuilder<ProductCatalogContext>();
        buider.UseSqlServer(connection);
        buider.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        buider.LogTo(s => Console.WriteLine(s), LogLevel.Information);
        var context = new ProductCatalogContext(buider.Options);

        ShowBrands(context);
        connection.Close();
    }

    private static void LeanAndMean()
    {
        var buider = new DbContextOptionsBuilder<ProductCatalogContext>();
        buider.UseSqlServer(conStr);
        var context = new ProductCatalogContext(buider.Options);

        ShowBrands(context);
    }

    public static void ShowBrands(ProductCatalogContext context)
    {
        foreach(var brand in context.Brands)
        {
            Console.WriteLine($"[{brand.Id}] {brand.Name}");
        }
    }
}
