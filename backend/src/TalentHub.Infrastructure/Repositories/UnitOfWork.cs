using TalentHub.Domain.Interfaces;
using TalentHub.Infrastructure.Data;

namespace TalentHub.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly TalentHubDbContext _context;

    public UnitOfWork(TalentHubDbContext context)
    {
        _context = context;
        Companies = new CompanyRepository(context);
        Jobs = new JobRepository(context);
        Candidates = new CandidateRepository(context);
        Applications = new ApplicationRepository(context);
        Categories = new CategoryRepository(context);
        Skills = new SkillRepository(context);
        Subscriptions = new SubscriptionRepository(context);
        Payments = new PaymentRepository(context);
        Notifications = new NotificationRepository(context);
        JobPostUsages = new JobPostUsageRepository(context);
        Users = new UserRepository(context);
        Plans = new Repository<Domain.Entities.Plan>(context);
    }

    public ICompanyRepository Companies { get; }
    public IJobRepository Jobs { get; }
    public ICandidateRepository Candidates { get; }
    public IApplicationRepository Applications { get; }
    public ICategoryRepository Categories { get; }
    public ISkillRepository Skills { get; }
    public ISubscriptionRepository Subscriptions { get; }
    public IPaymentRepository Payments { get; }
    public INotificationRepository Notifications { get; }
    public IJobPostUsageRepository JobPostUsages { get; }
    public IUserRepository Users { get; }
    public IRepository<Domain.Entities.Plan> Plans { get; }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await _context.SaveChangesAsync(cancellationToken);

    public void Dispose() => _context.Dispose();
}
