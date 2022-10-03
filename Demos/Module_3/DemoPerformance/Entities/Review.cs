namespace DemoPerformance;

public class Review: Entity
{
    public long ProductId { get; set; }
    public string? Text { get; set; }
    public byte Score { get; set; }
    public ReviewType ReviewType { get; set; }
    public long? ReviewerId { get; set; }

    public virtual Reviewer? Reviewer { get; set; }
    public virtual Product? Product { get; set; } = null!;
}
