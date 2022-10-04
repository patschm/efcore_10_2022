using System;
using System.Collections.Generic;

namespace Scaffold
{
    public partial class Review
    {
        public long Id { get; set; }
        public string? Text { get; set; }
        public byte Score { get; set; }
        public int ReviewType { get; set; }
        public long ProductId { get; set; }
        public long? ReviewerId { get; set; }
        public DateTime? DateBought { get; set; }
        public string? Organization { get; set; }
        public string? ReviewUrl { get; set; }
        public byte[]? Timestamp { get; set; }

        public virtual Product Product { get; set; } = null!;
        public virtual Reviewer? Reviewer { get; set; }
    }
}
