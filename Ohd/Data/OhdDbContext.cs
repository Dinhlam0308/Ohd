using Microsoft.EntityFrameworkCore;
using Ohd.Entities;

namespace Ohd.Data
{
    public class OhdDbContext : DbContext
    {
        public OhdDbContext(DbContextOptions<OhdDbContext> options)
            : base(options)
        {
        }

        // ==========================
        // Reference Tables
        // ==========================
        public DbSet<RequestStatus> RequestStatus { get; set; }
        public DbSet<Severity> severities { get; set; }
        public DbSet<Category> categories { get; set; }
        public DbSet<RequestPriority> request_priorities { get; set; }
        public DbSet<RequestStatusTransition> request_status_transitions { get; set; }
        public DbSet<Tags> tags { get; set; }
        public DbSet<Skills> skills { get; set; }
        public DbSet<SystemSetting> system_settings { get; set; }
         // ==========================
        // User & Auth
        // ==========================
        public DbSet<User> Users { get; set; }
        public DbSet<Role> roles { get; set; }
        public DbSet<Permission> permissions { get; set; }
        public DbSet<UserRoles> user_roles { get; set; }
        public DbSet<role_permissions> role_permissions { get; set; }
        public DbSet<password_reset_tokens> password_reset_tokens { get; set; }
        public DbSet<password_history> password_history { get; set; }
        public DbSet<user_sessions> user_sessions { get; set; }
        public DbSet<audit_logs> audit_logs { get; set; }
        public DbSet<MfaSecret> mfa_secrets { get; set; }
        public DbSet<ip_blocklist> ip_blocklist { get; set; }
        public DbSet<RateLimitBucket> rate_limit_buckets { get; set; }
        public DbSet<Timeline> timeline { get; set; }

        // ==========================
        // Domain: Requests Module
        // ==========================
        public DbSet<Facility> Facilities { get; set; }
        public DbSet<Request> requests { get; set; }
        public DbSet<Notification> notifications { get; set; }
        public DbSet<request_history> request_history { get; set; }
        public DbSet<Request_Comments> request_comments { get; set; }
        public DbSet<Attachment> attachments { get; set; }
        public DbSet<RequestCategory> request_categories { get; set; }
        public DbSet<sla_policies> sla_policies { get; set; }
        public DbSet<Escalation> escalations { get; set; }
        public DbSet<OutboxMessage> outbox_messages { get; set; }
        public DbSet<Teams> teams { get; set; }
        public DbSet<UserTeam> user_teams { get; set; }
        public DbSet<RequestTag> request_tags { get; set; }
        public DbSet<MaintenanceWindow> maintenance_windows { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ============================================================
            // UNIQUE
            // ============================================================
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // ============================================================
            // PRIMARY KEYS & COMPOSITE KEYS
            // ============================================================
            modelBuilder.Entity<SystemSetting>()
                .HasKey(x => x.key);

            modelBuilder.Entity<MfaSecret>()
                .HasKey(x => x.user_id);

            modelBuilder.Entity<UserRoles>()
                .HasKey(x => new { x.user_id, x.role_id });

            modelBuilder.Entity<role_permissions>()
                .HasKey(x => new { x.role_id, x.permission_id });

            modelBuilder.Entity<RequestStatusTransition>()
                .HasKey(x => new { x.from_status_id, x.to_status_id });

            modelBuilder.Entity<RequestCategory>()
                .HasKey(x => new { x.request_id, x.category_id });

            modelBuilder.Entity<UserTeam>()
                .HasKey(x => new { x.user_id, x.team_id });

            modelBuilder.Entity<RequestTag>()
                .HasKey(x => new { x.request_id, x.tag_id });
            modelBuilder.Entity<RequestStatus>().ToTable("request_statuses");
            // ============================================================
            // FACILITIES
            // ============================================================
            modelBuilder.Entity<Facility>().ToTable("facilities");

            modelBuilder.Entity<Facility>()
                .Property(f => f.Name).HasColumnName("name");

            modelBuilder.Entity<Facility>()
                .Property(f => f.Description).HasColumnName("description");

            modelBuilder.Entity<Facility>()
                .Property(f => f.HeadUserId).HasColumnName("head_user_id");

            modelBuilder.Entity<Facility>()
                .Property(f => f.CreatedAt).HasColumnName("created_at");

            // ============================================================
            // REQUESTS – FIX LỖI 500 (MAP FULL CỘT)
            // ============================================================
            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("requests");

                entity.Property(e => e.Id).HasColumnName("id");
                entity.Property(e => e.RequestorId).HasColumnName("requestor_id");
                entity.Property(e => e.FacilityId).HasColumnName("facility_id");
                entity.Property(e => e.AssigneeId).HasColumnName("assignee_id");
                entity.Property(e => e.Title).HasColumnName("title");
                entity.Property(e => e.Description).HasColumnName("description");
                entity.Property(e => e.SeverityId).HasColumnName("severity_id");
                entity.Property(e => e.StatusId).HasColumnName("status_id");
                entity.Property(e => e.PriorityId).HasColumnName("priority_id");
                entity.Property(e => e.Remarks).HasColumnName("remarks");
                entity.Property(e => e.CreatedAt).HasColumnName("created_at");
                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            // ============================================================
            // NOTIFICATIONS (nếu bạn dùng)
            // ============================================================
            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("notifications");

                entity.Property(e => e.id).HasColumnName("id");
                entity.Property(e => e.request_id).HasColumnName("request_id");
                entity.Property(e => e.message).HasColumnName("message");
                entity.Property(e => e.is_read).HasColumnName("is_read");
                entity.Property(e => e.created_at).HasColumnName("created_at");
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
