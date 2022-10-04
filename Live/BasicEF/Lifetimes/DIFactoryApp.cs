using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Scaffold;

internal class DIFactoryApp : IHostedService
{
    private readonly IDbContextFactory<ProductCatalogContext> _factory;

    public DIFactoryApp(IDbContextFactory<ProductCatalogContext> factory)
    {
        _factory = factory;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        using(var ctx = _factory.CreateDbContext())
        {
            Program.ShowBrands(ctx);
        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}