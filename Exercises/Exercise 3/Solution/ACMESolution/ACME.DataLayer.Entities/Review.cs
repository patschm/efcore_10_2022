namespace ACME.DataLayer.Entities;

// TODO 1: Create three new derived classes from Review
//               1) ConsumerReview and move the property DateBought there
//               2) ExpertReview and move the property Organization there
//               3) WebReview and move the  property ReviewUrl there
public class Review: Entity
{
    public string? Text { get; set; }
    public byte Score { get; set; }
    public ReviewType ReviewType { get; set; }
    public long ProductId { get; set; }

    public virtual Product? Product { get; set; } = null!;
    public virtual Reviewer? Reviewer { get; set; } = null!;
}
