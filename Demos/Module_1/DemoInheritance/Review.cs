namespace DemoInheritance;

public class Review
{
    public long Id { get; set; }
    public string? Text { get; set; }
    public int Score { get; set; }
    public ReviewType ReviewType { get; set; }

    public long ReviewerId { get; set; }
    public Reviewer? Reviewer { get; set; }
}
