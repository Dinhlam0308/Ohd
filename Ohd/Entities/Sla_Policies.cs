namespace Ohd.Entities
{
    public class sla_policies
    {
        public int id { get; set; }
        public int? facility_id { get; set; }
        public int? category_id { get; set; }
        public int priority { get; set; }
        public int respond_within_mins { get; set; }
        public int resolve_within_mins { get; set; }
        public bool active { get; set; }
        public DateTime created_at { get; set; }
    }
}