namespace ACME.Backend.ShopApi.Models;

public class ProductModel
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Brand { get; set; }
    public string? ProductGroup { get; set; }
    public string? Image { get; set; }
}
