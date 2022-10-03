using ACME.DataLayer.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACME.Frontend.ConsoleClient;

internal class ConsoleClient : IHostedService
{
    private readonly ShopDatabaseContext _dbContext;
    private static Func<ShopDatabaseContext, long, IEnumerable<ProductSpecification>> cquery =
        EF.CompileQuery((ShopDatabaseContext ctx, long id) => ctx.Specifications
                .Include(s => s.Product)
                    .ThenInclude(p => p.Brand)
                .Join(ctx.SpecificationDefinitions,
                    sd => sd.Key,
                    s => s.Key,
                   (s, sd) => new ProductSpecification
                   {
                       ProductId = s.ProductId,
                       ProductName = s.Product.Name,
                       Brand = s.Product.Brand.Name,
                       SpecName = sd.Name,
                       Type = sd.Type,
                       Unit = sd.Unit,
                       StringValue = s.StringValue,
                       BoolValue = s.BoolValue,
                       NumberValue = s.NumberValue
                   })
                .Where(p => p.ProductId == 1)
                .Select(p=>p)
        );

    public ConsoleClient(ShopDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        // TODO 1: Create a compiled query for the following query and test the program.
        //var data = _dbContext.Specifications
        //    .Include(s => s.Product)
        //        .ThenInclude(p => p.Brand)
        //    .Join(_dbContext.SpecificationDefinitions,
        //        sd => sd.Key,
        //        s => s.Key,
        //        (s, sd) => new ProductSpecification
        //        {
        //            ProductId = s.ProductId,
        //            ProductName = s.Product.Name,
        //            Brand = s.Product.Brand.Name,
        //            SpecName = sd.Name,
        //            Type = sd.Type,
        //            Unit = sd.Unit,
        //            StringValue = s.StringValue,
        //            BoolValue = s.BoolValue,
        //            NumberValue = s.NumberValue
        //        })
        //    .Where(p => p.ProductId == 1)
        //    .Select(p => p);

        var data = cquery.Invoke(_dbContext, 1);
        var query = data.ToList().GroupBy(p => new { p.ProductName, p.Brand });
        foreach(var item in query)
        {
            Console.WriteLine($"{item.Key.Brand} {item.Key.ProductName}");
            foreach(var spec in item)
            {
                Console.Write($"- {spec.SpecName} : ");
                if (spec.Type == "dialog" || spec.Type == "text")
                {
                    Console.Write($"{spec.StringValue}");
                }
                if (spec.Type == "yes_no")
                {
                    Console.Write($"{spec.BoolValue}");
                }
                if (spec.Type == "number")
                {
                    Console.Write($"{spec.NumberValue}");
                }
                Console.WriteLine($" {spec.Unit}");
            }
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
