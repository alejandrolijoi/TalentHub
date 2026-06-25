using TalentHub.Domain.Entities;
using TalentHub.Domain.Enums;

namespace TalentHub.Domain.Interfaces;

public interface ICompanyRepository : IRepository<Company>
{
    Task<Company?> GetByUserIdAsync(Guid userId);
    Task<Company?> GetByNameAsync(string name);
    Task<IReadOnlyList<Company>> SearchAsync(string? query, int page, int pageSize);
}

public interface IJobRepository : IRepository<Job>
{
    Task<Job?> GetWithDetailsAsync(Guid id);
    Task<IReadOnlyList<Job>> SearchAsync(string? query, Guid? categoryId, EmploymentType? type,
        ExperienceLevel? level, RemoteType? remote, decimal? salaryMin, decimal? salaryMax,
        string? location, int page, int pageSize);
    Task<IReadOnlyList<Job>> GetByCompanyIdAsync(Guid companyId, int page, int pageSize);
    Task<IReadOnlyList<Job>> GetFeaturedJobsAsync(int limit);
    Task<IReadOnlyList<Job>> GetByCandidateIdAsync(Guid candidateId);
    Task<int> GetMonthlyPostCountAsync(Guid companyId, int year, int month);
}

public interface ICandidateRepository : IRepository<Candidate>
{
    Task<Candidate?> GetByUserIdAsync(Guid userId);
    Task<Candidate?> GetWithDetailsAsync(Guid id);
}

public interface IApplicationRepository : IRepository<JobApplication>
{
    Task<JobApplication?> GetByJobAndCandidateAsync(Guid jobId, Guid candidateId);
    Task<IReadOnlyList<JobApplication>> GetByJobIdAsync(Guid jobId);
    Task<IReadOnlyList<JobApplication>> GetByCandidateIdAsync(Guid candidateId);
}

public interface ICategoryRepository : IRepository<Category>
{
    Task<Category?> GetBySlugAsync(string slug);
    Task<IReadOnlyList<Category>> GetWithJobCountsAsync();
}

public interface ISkillRepository : IRepository<Skill>
{
    Task<IReadOnlyList<Skill>> SearchAsync(string query);
}

public interface ISubscriptionRepository : IRepository<Subscription>
{
    Task<Subscription?> GetByCompanyIdAsync(Guid companyId);
    Task<Subscription?> GetByProviderSubscriptionIdAsync(string providerSubscriptionId);
}

public interface IPaymentRepository : IRepository<Payment>
{
    Task<IReadOnlyList<Payment>> GetByCompanyIdAsync(Guid companyId);
}

public interface INotificationRepository : IRepository<Notification>
{
    Task<IReadOnlyList<Notification>> GetByUserIdAsync(Guid userId, bool unreadOnly);
}

public interface IJobPostUsageRepository : IRepository<JobPostUsage>
{
    Task<JobPostUsage?> GetByCompanyAndMonthAsync(Guid companyId, int year, int month);
}
