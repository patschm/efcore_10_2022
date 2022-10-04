using System;
using System.Collections.Generic;

namespace Scaffold
{
    public partial class Reviewer
    {
        public Reviewer()
        {
            Reviews = new HashSet<Review>();
        }

        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? PasswordHash { get; set; }
        public string? PasswordSalt { get; set; }
        public byte[]? Timestamp { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
