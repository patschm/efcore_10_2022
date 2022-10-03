namespace DemoPerformance;

public class Reviewer : Entity
{
    public string? Name { get; set; }
    public string? Email { get; set; }

    public virtual ReviewerCredential? Credentials { get; set; }
    public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
}
