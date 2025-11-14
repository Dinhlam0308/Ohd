using System;

namespace Ohd.Entities
{
    public class MfaSecret
    {
        public long user_id { get; set; } // PRIMARY KEY
        public string type { get; set; } = null!;
        public byte[]? secret_encrypted { get; set; }
        public bool enabled { get; set; }
        public DateTime created_at { get; set; }

        // Navigation (optional)
        public User? user { get; set; }
    }
}