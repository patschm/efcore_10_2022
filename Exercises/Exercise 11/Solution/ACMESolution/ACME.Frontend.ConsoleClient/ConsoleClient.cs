using ACME.DataLayer.Repository.SqlServer;
using Microsoft.EntityFrameworkCore;
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

    public ConsoleClient(ShopDatabaseContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var query = _dbContext.Brands
                                .Include(b => b.Products)
                                    .ThenInclude(p => p.ProductGroup)
                                .OrderBy(p=>p.Name)
                                .Take(3);
        foreach(var record in query)
        {
            Console.WriteLine($"{record.Name}");
            foreach(var product in record.Products)
            {
                Console.WriteLine($"\t[{product.ProductGroup.Name}] {product.Name}");
            }
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
