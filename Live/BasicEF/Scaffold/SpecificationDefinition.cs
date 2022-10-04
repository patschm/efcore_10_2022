using System;
using System.Collections.Generic;

namespace Scaffold
{
    public partial class SpecificationDefinition
    {
        public long Id { get; set; }
        public string Key { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Unit { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public long ProductGroupId { get; set; }
        public byte[]? Timestamp { get; set; }

        public virtual ProductGroup ProductGroup { get; set; } = null!;
    }
}
