namespace DemoTransactions;

public class ProductGroup
{
    public ProductGroup()
    {
        Products = new HashSet<Product>();
    }

    public long Id { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    
    public virtual ICollection<Product> Products { get; set; }
}
