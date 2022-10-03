using ACME.Backend.ShopApi.Models;
using ACME.DataLayer.Entities;
using ACME.DataLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ACME.Backend.ShopApi.Controllers;

[ApiController]
[Route("[controller]")]
public class BrandController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<BrandController> _logger;

    public BrandController(IUnitOfWork uow, ILogger<BrandController> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<BrandModel>> GetAsync(int page = 1,  int count = 20)
    {
        var result = await _uow.Brands.GetAllAsync(page, count);
        return result.Select(p => new BrandModel { Id = p.Id, Name = p.Name, Website = p.Website });
    }
    [HttpGet("{id}")]
    public async Task<BrandModel> GetByIdAsync(long id)
    {
        var result = await _uow.Brands.GetByIdAsync(id);
        return new BrandModel { Id = result.Id, Name = result.Name, Website=result.Website };

    }
    [HttpGet("{id}/Products")]
    public async Task<IEnumerable<ProductModel>> GetProductsByBrandIdAsync(long id, int page = 1, int count=10)
    {
        var result = await _uow.Brands.GetProductsAsync(id, page, count);
        return result.Select(p=>new ProductModel { 
                Id = p.Id,
                Name = p.Name,
                Brand = p.Brand.Name,
                Image = p.Image,
                ProductGroup = p.ProductGroup.Name
            });
    }
}