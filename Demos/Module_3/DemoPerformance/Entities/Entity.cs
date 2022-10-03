using System.ComponentModel.DataAnnotations;

namespace DemoPerformance;

public class Entity
{
    public long Id { get; set; }
    [Timestamp]
    public byte[]? Timestamp { get; set; }
}
