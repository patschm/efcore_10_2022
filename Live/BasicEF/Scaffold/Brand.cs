using System;
using System.Collections.Generic;

namespace Scaffold
{
    public partial class Brand
    {
        public Brand()
        {
            Products = new HashSet<Product>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Website { get; set; }
        public byte[]? Timestamp { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
