using System;
using System.Collections.Generic;

namespace Scaffold
{
    public partial class ProductGroup
    {
        public ProductGroup()
        {
            Products = new HashSet<Product>();
            SpecificationDefinitions = new HashSet<SpecificationDefinition>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public byte[]? Timestamp { get; set; }

        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<SpecificationDefinition> SpecificationDefinitions { get; set; }
    }
}
