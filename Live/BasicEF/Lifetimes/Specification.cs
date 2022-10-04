using System;
using System.Collections.Generic;

namespace Scaffold
{
    public partial class Specification
    {
        public long Id { get; set; }
        public string Key { get; set; } = null!;
        public bool? BoolValue { get; set; }
        public string? StringValue { get; set; }
        public double? NumberValue { get; set; }
        public long ProductId { get; set; }
        public byte[]? Timestamp { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}
