using System;

namespace Ohd.Entities
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public byte[] Password_Hash { get; set; } = null!;
        public bool Is_First_Login { get; set; } = true;
        public bool Is_Active { get; set; } = true;
        public DateTime Created_At { get; set; }
        public DateTime? Password_Last_Changed_At { get; set; }
    }
}