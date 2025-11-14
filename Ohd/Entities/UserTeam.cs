namespace Ohd.Entities
{
    public class UserTeam
    {
        public long user_id { get; set; }
        public int team_id { get; set; }
        public DateTime joined_at { get; set; }
    }
}