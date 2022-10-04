
namespace BasicEF;

public class Person
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public ICollection<Hobby> Hobbies { get; set; } = new HashSet<Hobby>();
}
