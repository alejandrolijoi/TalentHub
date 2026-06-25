using TalentHub.Domain.Interfaces;
using TalentHub.Infrastructure.Data;
using TalentHub.Infrastructure.Repositories;
using TalentHub.Infrastructure.Services;
using TalentHub.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TalentHub.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<TalentHubDbContext>(options =>
            options.UseNpgsql(connectionString, b => b.MigrationsAssembly(typeof(TalentHubDbContext).Assembly.FullName)));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddScoped<ICompanyRepository, CompanyRepository>();
        services.AddScoped<IJobRepository, JobRepository>();
        services.AddScoped<ICandidateRepository, CandidateRepository>();
        services.AddScoped<IApplicationRepository, ApplicationRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ISkillRepository, SkillRepository>();
        services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();
        services.AddScoped<IJobPostUsageRepository, JobPostUsageRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<ISubscriptionService, SubscriptionService>();

        return services;
    }
}
