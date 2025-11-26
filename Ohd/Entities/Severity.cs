namespace Ohd.Entities
{
    public class Severity
    {
        public int id { get; set; }
        public string code { get; set; } = null!;
        public string name { get; set; } = null!;
        public int sort_order { get; set; }
        public bool auto_notify { get; set; } = false;                   // NEW
        public string? notify_email_template_code { get; set; }          // NEW
        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; } = DateTime.UtcNow;      // NEW
        
        public int FirstReminderMinutes { get; set; }   // thời gian gửi nhắc lần 1
        public int RepeatReminderMinutes { get; set; }  
    }
    
}