namespace Ohd.Entities
{
    public class password_history
    {
        public long id { get; set; }
        public long user_id { get; set; }
        public byte[] password_hash { get; set; } = null!;
        public DateTime changed_at { get; set; }
    }
}