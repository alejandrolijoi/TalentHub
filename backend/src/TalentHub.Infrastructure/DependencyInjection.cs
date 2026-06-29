using System;
using System.Text.RegularExpressions;
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
    private static string ParseConnectionString(string raw)
    {
        if (raw.StartsWith("postgres://") || raw.StartsWith("postgresql://"))
        {
            var uri = new Uri(raw);
            var userInfo = uri.UserInfo.Split(':');
            var host = uri.Host;
            var port = uri.Port > 0 ? uri.Port : 5432;
            var database = uri.AbsolutePath.TrimStart('/');
            var username = Uri.UnescapeDataString(userInfo[0]);
            var password = Uri.UnescapeDataString(userInfo[1]);
            return $"Host={host};Port={port};Database={database};Username={username};Password={password};SSL Mode=Require;Trust Server Certificate=true";
        }
        return raw;
    }

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var raw = configuration["ConnectionStrings:DefaultConnection"]
            ?? configuration["DATABASE_URL"]
            ?? throw new InvalidOperationException("Connection string not found. Set ConnectionStrings:DefaultConnection or DATABASE_URL.");

        var connectionString = ParseConnectionString(raw);

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
