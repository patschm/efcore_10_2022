using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scaffold;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BrandController : ControllerBase
    {
        private readonly ProductCatalogContext _catalogContext;
        private readonly ILogger<BrandController> _logger;

        public BrandController(ILogger<BrandController> logger, ProductCatalogContext catalogContext)
        {
            _logger = logger;
            _catalogContext = catalogContext;
            _catalogContext.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        [HttpGet(Name = "Brands")]
        public IEnumerable<Brand> Get()
        {
            return _catalogContext.Brands.AsNoTracking().ToList();             
        }
    }
}