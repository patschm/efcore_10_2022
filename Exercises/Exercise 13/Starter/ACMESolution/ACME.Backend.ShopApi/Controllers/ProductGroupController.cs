using ACME.Backend.ShopApi.Models;
using ACME.DataLayer.Entities;
using ACME.DataLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ACME.Backend.ShopApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductGroupController : ControllerBase
{
    // TODO 10: Inject the IUnitOfWork here.
    private readonly ILogger<ProductGroupController> _logger;

    public ProductGroupController(ILogger<ProductGroupController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<ProductGroupModel>> GetAsync(int page = 1,  int count = 20)
    {
        // TODO 11: Call the GetAllAsync from the ProductGroupRepository and assign it to result.
        IEnumerable<ProductGroup> result = default;
        return result.Select(p => new ProductGroupModel { Id = p.Id, Name = p.Name, Image = p.Image });
    }
    [HttpGet("{id}")]
    public async Task<ProductGroupModel> GetByIdAsync(long id)
    {
        // TODO 12: Call the GetByIdAsync from the ProductGroupRepository and assign it to result.
        ProductGroup result = default;
        return new ProductGroupModel { Id = result.Id, Name = result.Name, Image = result.Image };
    }
    [HttpGet("{id}/Products")]
    public async Task<IEnumerable<ProductModel>> GetProductsByGroupIdAsync(long id, int page = 1, int count = 10)
    {
        // TODO 13: Cal the GetProductsByGroupId and assign it to result..
        // Test the service. (Don't forget to set this project as startup project)
        IEnumerable<Product> result = default;
        return result.Select(p => new ProductModel
        {
            Id = p.Id,
            Name = p.Name,
            Brand = p.Brand.Name,
            Image = p.Image,
            ProductGroup = p.ProductGroup.Name
        });
    }
}