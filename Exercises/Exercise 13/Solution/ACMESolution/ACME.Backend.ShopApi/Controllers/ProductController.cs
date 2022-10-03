using ACME.Backend.ShopApi.Models;
using ACME.DataLayer.Entities;
using ACME.DataLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ACME.Backend.ShopApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly ILogger<ProductController> _logger;

    public ProductController(IUnitOfWork uow, ILogger<ProductController> logger)
    {
        _uow = uow;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<ProductModel>> GetAsync(int page = 1,  int count = 20)
    {
        var  result =  await _uow.Products.GetAllAsync(page, count);
        return result.Select(p => new ProductModel
        {
            Id = p.Id,
            Name = p.Name,
            Brand = p.Brand.Name,
            Image = p.Image,
            ProductGroup = p.ProductGroup.Name
        });
    }
    [HttpGet("{id}")]
    public async Task<ProductModel> GetByIdAsync(long id)
    {
        var p = await _uow.Products.GetByIdAsync(id);
        return  new ProductModel
        {
            Id = p.Id,
            Name = p.Name,
            Brand = p.Brand.Name,
            Image = p.Image,
            ProductGroup = p.ProductGroup.Name
        };
    }
}