using TalentHub.Domain.Entities;
using TalentHub.Domain.Interfaces;
using TalentHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TalentHub.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly TalentHubDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(TalentHubDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(Guid id) =>
        await _dbSet.FindAsync(id);

    public virtual async Task<IReadOnlyList<T>> GetAllAsync() =>
        await _dbSet.ToListAsync();

    public async Task<T> AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        return entity;
    }

    public Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task<int> CountAsync() =>
        await _dbSet.CountAsync();
}

public class CompanyRepository : Repository<Company>, ICompanyRepository
{
    public CompanyRepository(TalentHubDbContext context) : base(context) { }

    public async Task<Company?> GetByUserIdAsync(Guid userId) =>
        await _context.Companies.FirstOrDefaultAsync(c => c.UserId == userId);

    public async Task<Company?> GetByNameAsync(string name) =>
        await _context.Companies.FirstOrDefaultAsync(c => c.Name == name);

    public async Task<IReadOnlyList<Company>> SearchAsync(string? query, int page, int pageSize)
    {
        var q = _context.Companies.AsQueryable();
        if (!string.IsNullOrEmpty(query))
            q = q.Where(c => c.Name.Contains(query) || (c.Description != null && c.Description.Contains(query)));
        return await q.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }
}

public class JobRepository : Repository<Job>, IJobRepository
{
    public JobRepository(TalentHubDbContext context) : base(context) { }

    public async Task<Job?> GetWithDetailsAsync(Guid id) =>
        await _context.Jobs
            .Include(j => j.Company)
            .Include(j => j.Category)
            .Include(j => j.JobSkills).ThenInclude(js => js.Skill)
            .FirstOrDefaultAsync(j => j.Id == id);

    public async Task<IReadOnlyList<Job>> SearchAsync(
        string? query, Guid? categoryId, Domain.Enums.EmploymentType? type,
        Domain.Enums.ExperienceLevel? level, Domain.Enums.RemoteType? remote,
        decimal? salaryMin, decimal? salaryMax, string? location, int page, int pageSize)
    {
        var q = _context.Jobs
            .Include(j => j.Company)
            .Include(j => j.Category)
            .Include(j => j.JobSkills)
            .Where(j => j.Status == Domain.Enums.JobStatus.Active)
            .AsQueryable();

        if (!string.IsNullOrEmpty(query))
            q = q.Where(j => j.Title.Contains(query) || j.Description.Contains(query));
        if (categoryId.HasValue)
            q = q.Where(j => j.CategoryId == categoryId);
        if (type.HasValue)
            q = q.Where(j => j.EmploymentType == type);
        if (level.HasValue)
            q = q.Where(j => j.ExperienceLevel == level);
        if (remote.HasValue)
            q = q.Where(j => j.RemoteType == remote);
        if (salaryMin.HasValue)
            q = q.Where(j => j.SalaryMin >= salaryMin);
        if (salaryMax.HasValue)
            q = q.Where(j => j.SalaryMax <= salaryMax);
        if (!string.IsNullOrEmpty(location))
            q = q.Where(j => j.Location != null && j.Location.Contains(location));

        return await q.OrderByDescending(j => j.IsFeatured).ThenByDescending(j => j.CreatedAt)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
    }

    public async Task<IReadOnlyList<Job>> GetByCompanyIdAsync(Guid companyId, int page, int pageSize) =>
        await _context.Jobs
            .Include(j => j.Company)
            .Include(j => j.Category)
            .Where(j => j.CompanyId == companyId)
            .OrderByDescending(j => j.CreatedAt)
            .Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

    public async Task<IReadOnlyList<Job>> GetFeaturedJobsAsync(int limit) =>
        await _context.Jobs
            .Include(j => j.Company)
            .Where(j => j.IsFeatured && j.Status == Domain.Enums.JobStatus.Active)
            .OrderByDescending(j => j.CreatedAt)
            .Take(limit).ToListAsync();

    public async Task<IReadOnlyList<Job>> GetByCandidateIdAsync(Guid candidateId) =>
        await _context.JobApplications
            .Include(a => a.Job).ThenInclude(j => j.Company)
            .Where(a => a.CandidateId == candidateId)
            .Select(a => a.Job)
            .ToListAsync();

    public async Task<int> GetMonthlyPostCountAsync(Guid companyId, int year, int month) =>
        await _context.Jobs.CountAsync(j =>
            j.CompanyId == companyId &&
            j.CreatedAt.Year == year &&
            j.CreatedAt.Month == month);
}

public class CandidateRepository : Repository<Candidate>, ICandidateRepository
{
    public CandidateRepository(TalentHubDbContext context) : base(context) { }

    public async Task<Candidate?> GetByUserIdAsync(Guid userId) =>
        await _context.Candidates.FirstOrDefaultAsync(c => c.UserId == userId);

    public async Task<Candidate?> GetWithDetailsAsync(Guid id) =>
        await _context.Candidates
            .Include(c => c.User)
            .Include(c => c.CandidateSkills).ThenInclude(cs => cs.Skill)
            .FirstOrDefaultAsync(c => c.Id == id);
}

public class ApplicationRepository : Repository<JobApplication>, IApplicationRepository
{
    public ApplicationRepository(TalentHubDbContext context) : base(context) { }

    public async Task<JobApplication?> GetByJobAndCandidateAsync(Guid jobId, Guid candidateId) =>
        await _context.JobApplications.FirstOrDefaultAsync(a => a.JobId == jobId && a.CandidateId == candidateId);

    public async Task<IReadOnlyList<JobApplication>> GetByJobIdAsync(Guid jobId) =>
        await _context.JobApplications
            .Include(a => a.Candidate)
            .Where(a => a.JobId == jobId)
            .OrderByDescending(a => a.AppliedAt)
            .ToListAsync();

    public async Task<IReadOnlyList<JobApplication>> GetByCandidateIdAsync(Guid candidateId) =>
        await _context.JobApplications
            .Include(a => a.Job).ThenInclude(j => j.Company)
            .Where(a => a.CandidateId == candidateId)
            .OrderByDescending(a => a.AppliedAt)
            .ToListAsync();
}

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    public CategoryRepository(TalentHubDbContext context) : base(context) { }

    public async Task<Category?> GetBySlugAsync(string slug) =>
        await _context.Categories.FirstOrDefaultAsync(c => c.Slug == slug);

    public async Task<IReadOnlyList<Category>> GetWithJobCountsAsync() =>
        await _context.Categories.ToListAsync();
}

public class SkillRepository : Repository<Skill>, ISkillRepository
{
    public SkillRepository(TalentHubDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Skill>> SearchAsync(string query) =>
        await _context.Skills.Where(s => s.Name.Contains(query)).ToListAsync();
}

public class SubscriptionRepository : Repository<Subscription>, ISubscriptionRepository
{
    public SubscriptionRepository(TalentHubDbContext context) : base(context) { }

    public async Task<Subscription?> GetByCompanyIdAsync(Guid companyId) =>
        await _context.Subscriptions
            .Include(s => s.Plan)
            .FirstOrDefaultAsync(s => s.CompanyId == companyId);

    public async Task<Subscription?> GetByProviderSubscriptionIdAsync(string providerSubscriptionId) =>
        await _context.Subscriptions
            .Include(s => s.Plan)
            .FirstOrDefaultAsync(s => s.ProviderSubscriptionId == providerSubscriptionId);
}

public class PaymentRepository : Repository<Payment>, IPaymentRepository
{
    public PaymentRepository(TalentHubDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Payment>> GetByCompanyIdAsync(Guid companyId) =>
        await _context.Payments
            .Where(p => p.CompanyId == companyId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
}

public class NotificationRepository : Repository<Notification>, INotificationRepository
{
    public NotificationRepository(TalentHubDbContext context) : base(context) { }

    public async Task<IReadOnlyList<Notification>> GetByUserIdAsync(Guid userId, bool unreadOnly) =>
        await _context.Notifications
            .Where(n => n.UserId == userId && (!unreadOnly || !n.IsRead))
            .OrderByDescending(n => n.CreatedAt)
            .ToListAsync();
}

public class JobPostUsageRepository : Repository<JobPostUsage>, IJobPostUsageRepository
{
    public JobPostUsageRepository(TalentHubDbContext context) : base(context) { }

    public async Task<JobPostUsage?> GetByCompanyAndMonthAsync(Guid companyId, int year, int month) =>
        await _context.JobPostUsages
            .FirstOrDefaultAsync(u => u.CompanyId == companyId && u.Year == year && u.Month == month);
}

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(TalentHubDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
}
