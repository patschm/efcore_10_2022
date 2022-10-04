using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BasicEF;

internal class Program
{
    public const string conStr = @"Server=.\sqlexpress;Database=GameDb;Trusted_Connection=True;MultipleActiveResultSets=true;";

    static void Main(string[] args)
    {
        IHost host = CreateDefaultBuilder(args).Build();
        var bld = new DbContextOptionsBuilder();
        bld.UseSqlServer(conStr);
        var ctx = new DatabseContext(bld.Options);

        //ctx.Database.EnsureDeleted();
        //ctx.Database.EnsureCreated();
    }

    public static IHostBuilder CreateDefaultBuilder(string[] args) =>
         Host.CreateDefaultBuilder()
         .ConfigureServices(svcs => {
             svcs.AddDbContext<DatabseContext>(opts => {
                 opts.UseSqlServer(conStr);
             });
         });
}