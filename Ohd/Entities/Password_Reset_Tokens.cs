namespace Ohd.Entities
{
    public class password_reset_tokens
    {
        public long id { get; set; }
        public long user_id { get; set; }
        public byte[] token_hash { get; set; }
        public DateTime expires_at { get; set; }
        public DateTime? used_at { get; set; }
        public DateTime created_at { get; set; }
    }
}