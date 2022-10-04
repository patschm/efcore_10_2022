using System;
using System.Collections.Generic;

namespace Scaffold
{
    public partial class PriceHistory
    {
        public long Id { get; set; }
        public long? ProductId { get; set; }
        public double BasePrice { get; set; }
        public string ShopName { get; set; } = null!;
        public DateTime PriceDate { get; set; }
        public byte[]? Timestamp { get; set; }
    }
}
