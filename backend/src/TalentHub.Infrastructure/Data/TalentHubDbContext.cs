using TalentHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TalentHub.Infrastructure.Data;

public class TalentHubDbContext : DbContext
{
    public TalentHubDbContext(DbContextOptions<TalentHubDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Candidate> Candidates => Set<Candidate>();
    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Skill> Skills => Set<Skill>();
    public DbSet<JobSkill> JobSkills => Set<JobSkill>();
    public DbSet<CandidateSkill> CandidateSkills => Set<CandidateSkill>();
    public DbSet<SavedJob> SavedJobs => Set<SavedJob>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Subscription> Subscriptions => Set<Subscription>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<JobPostUsage> JobPostUsages => Set<JobPostUsage>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.Email).IsUnique();
            e.Property(u => u.Email).HasMaxLength(255);
        });

        modelBuilder.Entity<Candidate>(e =>
        {
            e.HasIndex(c => c.UserId).IsUnique();
            e.HasOne(c => c.User).WithOne(u => u.Candidate).HasForeignKey<Candidate>(c => c.UserId);
        });

        modelBuilder.Entity<Company>(e =>
        {
            e.HasIndex(c => c.UserId).IsUnique();
            e.HasOne(c => c.User).WithOne(u => u.Company).HasForeignKey<Company>(c => c.UserId);
        });

        modelBuilder.Entity<Job>(e =>
        {
            e.HasOne(j => j.Company).WithMany(c => c.Jobs).HasForeignKey(j => j.CompanyId);
            e.HasOne(j => j.Category).WithMany(c => c.Jobs).HasForeignKey(j => j.CategoryId);
            e.Property(j => j.Title).HasMaxLength(200);
            e.HasIndex(j => new { j.Status, j.CreatedAt });
        });

        modelBuilder.Entity<JobApplication>(e =>
        {
            e.HasIndex(a => new { a.JobId, a.CandidateId }).IsUnique();
            e.HasOne(a => a.Job).WithMany(j => j.Applications).HasForeignKey(a => a.JobId);
            e.HasOne(a => a.Candidate).WithMany(c => c.Applications).HasForeignKey(a => a.CandidateId);
        });

        modelBuilder.Entity<Category>(e =>
        {
            e.HasIndex(c => c.Slug).IsUnique();
            e.HasOne(c => c.Parent).WithMany(c => c.Children).HasForeignKey(c => c.ParentId);
        });

        modelBuilder.Entity<JobSkill>(e =>
        {
            e.HasKey(js => new { js.JobId, js.SkillId });
            e.HasOne(js => js.Job).WithMany(j => j.JobSkills).HasForeignKey(js => js.JobId);
            e.HasOne(js => js.Skill).WithMany(s => s.JobSkills).HasForeignKey(js => js.SkillId);
        });

        modelBuilder.Entity<CandidateSkill>(e =>
        {
            e.HasKey(cs => new { cs.CandidateId, cs.SkillId });
            e.HasOne(cs => cs.Candidate).WithMany(c => c.CandidateSkills).HasForeignKey(cs => cs.CandidateId);
            e.HasOne(cs => cs.Skill).WithMany(s => s.CandidateSkills).HasForeignKey(cs => cs.SkillId);
        });

        modelBuilder.Entity<SavedJob>(e =>
        {
            e.HasKey(sj => new { sj.CandidateId, sj.JobId });
            e.HasOne(sj => sj.Candidate).WithMany(c => c.SavedJobs).HasForeignKey(sj => sj.CandidateId);
            e.HasOne(sj => sj.Job).WithMany().HasForeignKey(sj => sj.JobId);
        });

        modelBuilder.Entity<Subscription>(e =>
        {
            e.HasIndex(s => s.CompanyId).IsUnique();
            e.HasOne(s => s.Company).WithOne(c => c.Subscription).HasForeignKey<Subscription>(s => s.CompanyId);
            e.HasOne(s => s.Plan).WithMany(p => p.Subscriptions).HasForeignKey(s => s.PlanId);
        });

        modelBuilder.Entity<Payment>(e =>
        {
            e.HasOne(p => p.Subscription).WithMany(s => s.Payments).HasForeignKey(p => p.SubscriptionId);
            e.HasOne(p => p.Company).WithMany(c => c.Payments).HasForeignKey(p => p.CompanyId);
        });

        modelBuilder.Entity<JobPostUsage>(e =>
        {
            e.HasIndex(u => new { u.CompanyId, u.Year, u.Month }).IsUnique();
            e.HasOne(u => u.Company).WithMany().HasForeignKey(u => u.CompanyId);
        });

        modelBuilder.Entity<Notification>(e =>
        {
            e.HasOne(n => n.User).WithMany().HasForeignKey(n => n.UserId);
            e.HasIndex(n => new { n.UserId, n.IsRead });
        });
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
        return base.SaveChangesAsync(cancellationToken);
    }
}
