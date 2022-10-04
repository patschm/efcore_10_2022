using Microsoft.Extensions.Hosting;
using Scaffold;

public class DIApp : IHostedService
{
    private readonly ProductCatalogContext _context;

    public DIApp(ProductCatalogContext context)
    {
        _context = context;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Program.ShowBrands(_context);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}