using TalentHub.Domain.Entities;

namespace TalentHub.Domain.Interfaces;

public interface IRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<int> CountAsync();
}

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
}

public interface IUnitOfWork : IDisposable
{
    ICompanyRepository Companies { get; }
    IJobRepository Jobs { get; }
    ICandidateRepository Candidates { get; }
    IApplicationRepository Applications { get; }
    ICategoryRepository Categories { get; }
    ISkillRepository Skills { get; }
    ISubscriptionRepository Subscriptions { get; }
    IPaymentRepository Payments { get; }
    INotificationRepository Notifications { get; }
    IJobPostUsageRepository JobPostUsages { get; }
    IUserRepository Users { get; }
    IRepository<Plan> Plans { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
