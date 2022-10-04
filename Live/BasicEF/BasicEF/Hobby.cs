using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicEF;

public class Hobby
{
    public int Id { get; set; }
    public string? Description { get; set; }
    public ICollection<Person> People { get; set; } = new HashSet<Person>();
}
