namespace DemoPerformance;

public class ReviewerCredential: Entity
{
    public string? UserName { get; set; }
    public string? PasswordHash { get; set; }
    public string? PasswordSalt { get; set; }
}
