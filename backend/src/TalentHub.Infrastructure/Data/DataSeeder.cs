using TalentHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TalentHub.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(TalentHubDbContext db)
    {
        if (await db.Plans.AnyAsync()) return;

        var plans = new List<Plan>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Free",
                PriceMonthly = 0,
                PriceYearly = 0,
                Currency = "USD",
                MaxJobsPerMonth = 3,
                MaxApplicantsPerJob = 25,
                FeaturesJson = """{"basicAnalytics":true,"emailSupport":true}""",
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Pro",
                PriceMonthly = 49,
                PriceYearly = 470,
                Currency = "USD",
                MaxJobsPerMonth = 15,
                MaxApplicantsPerJob = 100,
                FeaturesJson = """{"advancedAnalytics":true,"prioritySupport":true,"featuredJobs":true}""",
                IsActive = true
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Enterprise",
                PriceMonthly = 149,
                PriceYearly = 1430,
                Currency = "USD",
                MaxJobsPerMonth = -1,
                MaxApplicantsPerJob = null,
                FeaturesJson = """{"unlimited":true,"dedicatedSupport":true,"customBranding":true,"apiAccess":true}""",
                IsActive = true
            }
        };

        var categories = new List<Category>
        {
            new() { Id = Guid.NewGuid(), Name = "Software Development", Slug = "software-development", Icon = "code", JobCount = 0 },
            new() { Id = Guid.NewGuid(), Name = "Data Science", Slug = "data-science", Icon = "bar_chart", JobCount = 0 },
            new() { Id = Guid.NewGuid(), Name = "Design", Slug = "design", Icon = "palette", JobCount = 0 },
            new() { Id = Guid.NewGuid(), Name = "Marketing", Slug = "marketing", Icon = "campaign", JobCount = 0 },
            new() { Id = Guid.NewGuid(), Name = "Sales", Slug = "sales", Icon = "trending_up", JobCount = 0 },
            new() { Id = Guid.NewGuid(), Name = "DevOps", Slug = "devops", Icon = "cloud", JobCount = 0 },
            new() { Id = Guid.NewGuid(), Name = "Product Management", Slug = "product-management", Icon = "rocket_launch", JobCount = 0 },
            new() { Id = Guid.NewGuid(), Name = "Finance", Slug = "finance", Icon = "account_balance", JobCount = 0 },
            new() { Id = Guid.NewGuid(), Name = "Human Resources", Slug = "human-resources", Icon = "people", JobCount = 0 },
            new() { Id = Guid.NewGuid(), Name = "Customer Support", Slug = "customer-support", Icon = "headset_mic", JobCount = 0 }
        };

        var skills = new List<Skill>
        {
            new() { Id = Guid.NewGuid(), Name = "C#", Category = "Backend" },
            new() { Id = Guid.NewGuid(), Name = ".NET", Category = "Backend" },
            new() { Id = Guid.NewGuid(), Name = "ASP.NET Core", Category = "Backend" },
            new() { Id = Guid.NewGuid(), Name = "Entity Framework", Category = "Backend" },
            new() { Id = Guid.NewGuid(), Name = "SQL Server", Category = "Database" },
            new() { Id = Guid.NewGuid(), Name = "PostgreSQL", Category = "Database" },
            new() { Id = Guid.NewGuid(), Name = "MongoDB", Category = "Database" },
            new() { Id = Guid.NewGuid(), Name = "Redis", Category = "Database" },
            new() { Id = Guid.NewGuid(), Name = "Docker", Category = "DevOps" },
            new() { Id = Guid.NewGuid(), Name = "Kubernetes", Category = "DevOps" },
            new() { Id = Guid.NewGuid(), Name = "AWS", Category = "Cloud" },
            new() { Id = Guid.NewGuid(), Name = "Azure", Category = "Cloud" },
            new() { Id = Guid.NewGuid(), Name = "GCP", Category = "Cloud" },
            new() { Id = Guid.NewGuid(), Name = "React", Category = "Frontend" },
            new() { Id = Guid.NewGuid(), Name = "Next.js", Category = "Frontend" },
            new() { Id = Guid.NewGuid(), Name = "TypeScript", Category = "Frontend" },
            new() { Id = Guid.NewGuid(), Name = "Tailwind CSS", Category = "Frontend" },
            new() { Id = Guid.NewGuid(), Name = "Angular", Category = "Frontend" },
            new() { Id = Guid.NewGuid(), Name = "Vue.js", Category = "Frontend" },
            new() { Id = Guid.NewGuid(), Name = "Python", Category = "Backend" },
            new() { Id = Guid.NewGuid(), Name = "Node.js", Category = "Backend" },
            new() { Id = Guid.NewGuid(), Name = "JavaScript", Category = "Frontend" },
            new() { Id = Guid.NewGuid(), Name = "Git", Category = "Tools" },
            new() { Id = Guid.NewGuid(), Name = "CI/CD", Category = "DevOps" },
            new() { Id = Guid.NewGuid(), Name = "Terraform", Category = "DevOps" },
            new() { Id = Guid.NewGuid(), Name = "GraphQL", Category = "API" },
            new() { Id = Guid.NewGuid(), Name = "REST APIs", Category = "API" },
            new() { Id = Guid.NewGuid(), Name = "RabbitMQ", Category = "Messaging" },
            new() { Id = Guid.NewGuid(), Name = "Kafka", Category = "Messaging" },
            new() { Id = Guid.NewGuid(), Name = "Microservices", Category = "Architecture" }
        };

        db.Plans.AddRange(plans);
        db.Categories.AddRange(categories);
        db.Skills.AddRange(skills);
        await db.SaveChangesAsync();
    }
}
