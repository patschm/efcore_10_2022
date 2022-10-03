using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace DemoLifetime
{
    internal class DIPoolingApp : IHostedService
    {
        private ProductContext _context;

        public DIPoolingApp(ProductContext context)
        {
            _context = context;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var brand in _context.Brands)
            {
                Console.WriteLine(brand.Name);
            }
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}