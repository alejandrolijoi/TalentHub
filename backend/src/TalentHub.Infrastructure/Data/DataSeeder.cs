using BCrypt.Net;
using Microsoft.EntityFrameworkCore;
using TalentHub.Domain.Entities;
using TalentHub.Domain.Enums;

namespace TalentHub.Infrastructure.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(TalentHubDbContext db)
    {
        await SeedPlansAsync(db);
        await SeedCategoriesAsync(db);
        await SeedSkillsAsync(db);
        await SeedDemoUsersCompaniesJobsAndApplicationsAsync(db);
    }

    private static async Task SeedPlansAsync(TalentHubDbContext db)
    {
        if (await db.Plans.AnyAsync()) return;

        db.Plans.AddRange(
            new Plan
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
            new Plan
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
            new Plan
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
            });

        await db.SaveChangesAsync();
    }

    private static async Task SeedCategoriesAsync(TalentHubDbContext db)
    {
        var seeds = new[]
        {
            new Category { Id = Guid.NewGuid(), Name = "Software Development", Slug = "software-development", Icon = "code", JobCount = 0 },
            new Category { Id = Guid.NewGuid(), Name = "Data Science", Slug = "data-science", Icon = "bar_chart", JobCount = 0 },
            new Category { Id = Guid.NewGuid(), Name = "Design", Slug = "design", Icon = "palette", JobCount = 0 },
            new Category { Id = Guid.NewGuid(), Name = "Marketing", Slug = "marketing", Icon = "campaign", JobCount = 0 },
            new Category { Id = Guid.NewGuid(), Name = "Sales", Slug = "sales", Icon = "trending_up", JobCount = 0 },
            new Category { Id = Guid.NewGuid(), Name = "DevOps", Slug = "devops", Icon = "cloud", JobCount = 0 },
            new Category { Id = Guid.NewGuid(), Name = "Product Management", Slug = "product-management", Icon = "rocket_launch", JobCount = 0 },
            new Category { Id = Guid.NewGuid(), Name = "Finance", Slug = "finance", Icon = "account_balance", JobCount = 0 },
            new Category { Id = Guid.NewGuid(), Name = "Human Resources", Slug = "human-resources", Icon = "people", JobCount = 0 },
            new Category { Id = Guid.NewGuid(), Name = "Customer Support", Slug = "customer-support", Icon = "headset_mic", JobCount = 0 }
        };

        var existing = await db.Categories.ToDictionaryAsync(c => c.Slug);
        var added = false;

        foreach (var seed in seeds)
        {
            if (existing.ContainsKey(seed.Slug)) continue;
            db.Categories.Add(seed);
            existing[seed.Slug] = seed;
            added = true;
        }

        if (added) await db.SaveChangesAsync();
    }

    private static async Task SeedSkillsAsync(TalentHubDbContext db)
    {
        var seeds = new[]
        {
            new Skill { Id = Guid.NewGuid(), Name = "C#", Category = "Backend" },
            new Skill { Id = Guid.NewGuid(), Name = ".NET", Category = "Backend" },
            new Skill { Id = Guid.NewGuid(), Name = "ASP.NET Core", Category = "Backend" },
            new Skill { Id = Guid.NewGuid(), Name = "Entity Framework", Category = "Backend" },
            new Skill { Id = Guid.NewGuid(), Name = "SQL Server", Category = "Database" },
            new Skill { Id = Guid.NewGuid(), Name = "PostgreSQL", Category = "Database" },
            new Skill { Id = Guid.NewGuid(), Name = "MongoDB", Category = "Database" },
            new Skill { Id = Guid.NewGuid(), Name = "Redis", Category = "Database" },
            new Skill { Id = Guid.NewGuid(), Name = "Docker", Category = "DevOps" },
            new Skill { Id = Guid.NewGuid(), Name = "Kubernetes", Category = "DevOps" },
            new Skill { Id = Guid.NewGuid(), Name = "AWS", Category = "Cloud" },
            new Skill { Id = Guid.NewGuid(), Name = "Azure", Category = "Cloud" },
            new Skill { Id = Guid.NewGuid(), Name = "GCP", Category = "Cloud" },
            new Skill { Id = Guid.NewGuid(), Name = "React", Category = "Frontend" },
            new Skill { Id = Guid.NewGuid(), Name = "Next.js", Category = "Frontend" },
            new Skill { Id = Guid.NewGuid(), Name = "TypeScript", Category = "Frontend" },
            new Skill { Id = Guid.NewGuid(), Name = "Tailwind CSS", Category = "Frontend" },
            new Skill { Id = Guid.NewGuid(), Name = "Angular", Category = "Frontend" },
            new Skill { Id = Guid.NewGuid(), Name = "Vue.js", Category = "Frontend" },
            new Skill { Id = Guid.NewGuid(), Name = "Python", Category = "Backend" },
            new Skill { Id = Guid.NewGuid(), Name = "Node.js", Category = "Backend" },
            new Skill { Id = Guid.NewGuid(), Name = "JavaScript", Category = "Frontend" },
            new Skill { Id = Guid.NewGuid(), Name = "Git", Category = "Tools" },
            new Skill { Id = Guid.NewGuid(), Name = "CI/CD", Category = "DevOps" },
            new Skill { Id = Guid.NewGuid(), Name = "Terraform", Category = "DevOps" },
            new Skill { Id = Guid.NewGuid(), Name = "GraphQL", Category = "API" },
            new Skill { Id = Guid.NewGuid(), Name = "REST APIs", Category = "API" },
            new Skill { Id = Guid.NewGuid(), Name = "RabbitMQ", Category = "Messaging" },
            new Skill { Id = Guid.NewGuid(), Name = "Kafka", Category = "Messaging" },
            new Skill { Id = Guid.NewGuid(), Name = "Microservices", Category = "Architecture" }
        };

        var existing = await db.Skills.ToDictionaryAsync(s => s.Name);
        var added = false;

        foreach (var seed in seeds)
        {
            if (existing.ContainsKey(seed.Name)) continue;
            db.Skills.Add(seed);
            existing[seed.Name] = seed;
            added = true;
        }

        if (added) await db.SaveChangesAsync();
    }

    private static async Task SeedDemoUsersCompaniesJobsAndApplicationsAsync(TalentHubDbContext db)
    {
        var now = DateTime.UtcNow;

        var categories = await db.Categories.ToDictionaryAsync(c => c.Slug);
        var skills = await db.Skills.ToDictionaryAsync(s => s.Name);

        var companySeeds = new[]
        {
            new CompanySeed(
                Email: "careers@acmecloud.dev",
                Password: "TalentHub123!",
                FirstName: "Mia",
                LastName: "Stone",
                CompanyName: "Acme Cloud",
                Description: "Cloud-native platform helping teams ship internal tools faster.",
                LogoUrl: "https://ui-avatars.com/api/?name=Acme+Cloud&background=0f172a&color=fff",
                Website: "https://acmecloud.dev",
                LinkedInUrl: "https://linkedin.com/company/acmecloud",
                Industry: "SaaS",
                CompanySize: "51-200",
                Location: "Remote / US",
                FoundedYear: 2019),
            new CompanySeed(
                Email: "jobs@pixelforge.dev",
                Password: "TalentHub123!",
                FirstName: "Diego",
                LastName: "Ramos",
                CompanyName: "PixelForge Studios",
                Description: "Product studio building polished web apps and design systems.",
                LogoUrl: "https://ui-avatars.com/api/?name=PixelForge&background=7c3aed&color=fff",
                Website: "https://pixelforge.dev",
                LinkedInUrl: "https://linkedin.com/company/pixelforge",
                Industry: "Digital Product Studio",
                CompanySize: "11-50",
                Location: "Buenos Aires, AR",
                FoundedYear: 2020),
            new CompanySeed(
                Email: "hello@datanest.io",
                Password: "TalentHub123!",
                FirstName: "Sofia",
                LastName: "Perez",
                CompanyName: "DataNest",
                Description: "Analytics and data engineering for fast-growing startups.",
                LogoUrl: "https://ui-avatars.com/api/?name=DataNest&background=059669&color=fff",
                Website: "https://datanest.io",
                LinkedInUrl: "https://linkedin.com/company/datanest",
                Industry: "Data & Analytics",
                CompanySize: "11-50",
                Location: "Remote / LATAM",
                FoundedYear: 2018)
        };

        foreach (var seed in companySeeds)
        {
            var email = seed.Email.ToLowerInvariant();
            var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                user = new User
                {
                    Id = Guid.NewGuid(),
                    Email = email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(seed.Password),
                    Role = UserRole.Company,
                    EmailConfirmed = true,
                    AvatarUrl = seed.LogoUrl,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                db.Users.Add(user);
            }

            var company = await db.Companies.FirstOrDefaultAsync(c => c.UserId == user.Id);
            if (company == null)
            {
                company = new Company
                {
                    Id = Guid.NewGuid(),
                    UserId = user.Id,
                    Name = seed.CompanyName,
                    Description = seed.Description,
                    LogoUrl = seed.LogoUrl,
                    Website = seed.Website,
                    LinkedInUrl = seed.LinkedInUrl,
                    Industry = seed.Industry,
                    CompanySize = seed.CompanySize,
                    Location = seed.Location,
                    FoundedYear = seed.FoundedYear,
                    CreatedAt = now,
                    UpdatedAt = now
                };
                db.Companies.Add(company);
            }
        }

        var candidateEmail = "candidate@talenthub.dev";
        var candidateUser = await db.Users.FirstOrDefaultAsync(u => u.Email == candidateEmail);
        if (candidateUser == null)
        {
            candidateUser = new User
            {
                Id = Guid.NewGuid(),
                Email = candidateEmail,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("TalentHub123!"),
                Role = UserRole.Candidate,
                EmailConfirmed = true,
                AvatarUrl = "https://ui-avatars.com/api/?name=Talent+Hub&background=2563eb&color=fff",
                CreatedAt = now,
                UpdatedAt = now
            };
            db.Users.Add(candidateUser);
        }

        var candidate = await db.Candidates.FirstOrDefaultAsync(c => c.UserId == candidateUser.Id);
        if (candidate == null)
        {
            candidate = new Candidate
            {
                Id = Guid.NewGuid(),
                UserId = candidateUser.Id,
                FirstName = "Talent",
                LastName = "Hub",
                Title = "Full Stack Developer",
                Bio = "Product-minded developer focused on .NET, React and cloud systems.",
                Location = "Montevideo, UY",
                YearsExperience = 4,
                LinkedInUrl = "https://linkedin.com/in/talenthub",
                GithubUrl = "https://github.com/talenthub",
                Website = "https://talenthub.dev",
                CreatedAt = now,
                UpdatedAt = now
            };
            db.Candidates.Add(candidate);
        }

        await db.SaveChangesAsync();

        var companiesByName = await db.Companies.ToDictionaryAsync(c => c.Name);

        var jobSeeds = new[]
        {
            new JobSeed(
                Title: "Senior .NET Engineer",
                CompanyName: "Acme Cloud",
                CategorySlug: "software-development",
                Description: "Build APIs and internal tools for a fast-growing SaaS platform.",
                Requirements: "5+ years with C#, ASP.NET Core, PostgreSQL, and microservices.",
                Benefits: "Remote-first, flexible hours, learning budget.",
                EmploymentType: EmploymentType.FullTime,
                ExperienceLevel: ExperienceLevel.Senior,
                Location: "Remote",
                RemoteType: RemoteType.Remote,
                SalaryMin: 90000,
                SalaryMax: 125000,
                Currency: "USD",
                SkillNames: new[] { "C#", ".NET", "ASP.NET Core", "PostgreSQL", "Docker", "Microservices" },
                IsFeatured: true,
                DaysAgo: 2,
                ViewCount: 128,
                ApplicationCount: 18),
            new JobSeed(
                Title: "DevOps Engineer",
                CompanyName: "Acme Cloud",
                CategorySlug: "devops",
                Description: "Own CI/CD pipelines, Kubernetes deployments and cloud automation.",
                Requirements: "Strong AWS, Terraform, Docker and Kubernetes experience.",
                Benefits: "Remote-first, high ownership, strong engineering culture.",
                EmploymentType: EmploymentType.FullTime,
                ExperienceLevel: ExperienceLevel.Senior,
                Location: "Remote",
                RemoteType: RemoteType.Remote,
                SalaryMin: 85000,
                SalaryMax: 120000,
                Currency: "USD",
                SkillNames: new[] { "AWS", "Terraform", "Docker", "Kubernetes", "CI/CD" },
                IsFeatured: true,
                DaysAgo: 4,
                ViewCount: 94,
                ApplicationCount: 11),
            new JobSeed(
                Title: "Frontend Engineer",
                CompanyName: "PixelForge Studios",
                CategorySlug: "software-development",
                Description: "Build polished client experiences for modern product teams.",
                Requirements: "React, Next.js, TypeScript and strong UI craftsmanship.",
                Benefits: "Hybrid schedule, design-led culture, growth opportunities.",
                EmploymentType: EmploymentType.FullTime,
                ExperienceLevel: ExperienceLevel.Mid,
                Location: "Buenos Aires, AR",
                RemoteType: RemoteType.Hybrid,
                SalaryMin: 65000,
                SalaryMax: 95000,
                Currency: "USD",
                SkillNames: new[] { "React", "Next.js", "TypeScript", "Tailwind CSS", "JavaScript" },
                IsFeatured: true,
                DaysAgo: 6,
                ViewCount: 84,
                ApplicationCount: 9),
            new JobSeed(
                Title: "Data Engineer",
                CompanyName: "DataNest",
                CategorySlug: "data-science",
                Description: "Design reliable data pipelines for product analytics and reporting.",
                Requirements: "Python, SQL, cloud warehouses and ETL/ELT experience.",
                Benefits: "Remote, great learning budget, modern data stack.",
                EmploymentType: EmploymentType.FullTime,
                ExperienceLevel: ExperienceLevel.Mid,
                Location: "Remote / LATAM",
                RemoteType: RemoteType.Remote,
                SalaryMin: 70000,
                SalaryMax: 100000,
                Currency: "USD",
                SkillNames: new[] { "Python", "PostgreSQL", "AWS", "Docker", "REST APIs" },
                IsFeatured: false,
                DaysAgo: 8,
                ViewCount: 65,
                ApplicationCount: 7),
            new JobSeed(
                Title: "Product Designer",
                CompanyName: "PixelForge Studios",
                CategorySlug: "design",
                Description: "Shape the design system and user flows for B2B web apps.",
                Requirements: "3+ years in product design, Figma, systems thinking.",
                Benefits: "Hybrid schedule, cross-functional team, design ownership.",
                EmploymentType: EmploymentType.FullTime,
                ExperienceLevel: ExperienceLevel.Mid,
                Location: "Buenos Aires, AR",
                RemoteType: RemoteType.Hybrid,
                SalaryMin: 60000,
                SalaryMax: 90000,
                Currency: "USD",
                SkillNames: new[] { "Git", "JavaScript", "React" },
                IsFeatured: false,
                DaysAgo: 10,
                ViewCount: 41,
                ApplicationCount: 4)
        };

        foreach (var seed in jobSeeds)
        {
            if (!companiesByName.TryGetValue(seed.CompanyName, out var company)) continue;

            var existingJob = await db.Jobs.FirstOrDefaultAsync(j => j.CompanyId == company.Id && j.Title == seed.Title);
            if (existingJob != null) continue;

            if (!categories.TryGetValue(seed.CategorySlug, out var category))
            {
                throw new InvalidOperationException($"Missing category '{seed.CategorySlug}' required for seeded jobs.");
            }

            var job = new Job
            {
                Id = Guid.NewGuid(),
                CompanyId = company.Id,
                Title = seed.Title,
                Description = seed.Description,
                Requirements = seed.Requirements,
                Benefits = seed.Benefits,
                CategoryId = category.Id,
                EmploymentType = seed.EmploymentType,
                ExperienceLevel = seed.ExperienceLevel,
                Location = seed.Location,
                RemoteType = seed.RemoteType,
                SalaryMin = seed.SalaryMin,
                SalaryMax = seed.SalaryMax,
                Currency = seed.Currency,
                Status = JobStatus.Active,
                IsFeatured = seed.IsFeatured,
                FeaturedUntil = seed.IsFeatured ? now.AddDays(30) : null,
                ExpiresAt = now.AddDays(60),
                ViewCount = seed.ViewCount,
                ApplicationCount = seed.ApplicationCount,
                CreatedAt = now.AddDays(-seed.DaysAgo),
                UpdatedAt = now.AddDays(-seed.DaysAgo)
            };

            foreach (var skillName in seed.SkillNames)
            {
                if (!skills.TryGetValue(skillName, out var skill))
                {
                    throw new InvalidOperationException($"Missing skill '{skillName}' required for seeded jobs.");
                }

                job.JobSkills.Add(new JobSkill
                {
                    JobId = job.Id,
                    SkillId = skill.Id,
                    IsRequired = true
                });
            }

            db.Jobs.Add(job);
        }

        await db.SaveChangesAsync();

        var seededJob = await db.Jobs.FirstOrDefaultAsync(j => j.Title == "Senior .NET Engineer");
        if (seededJob != null)
        {
            var applicationExists = await db.JobApplications.AnyAsync(a => a.JobId == seededJob.Id && a.CandidateId == candidate.Id);
            if (!applicationExists)
            {
                db.JobApplications.Add(new JobApplication
                {
                    Id = Guid.NewGuid(),
                    JobId = seededJob.Id,
                    CandidateId = candidate.Id,
                    CoverLetter = "I love building scalable SaaS products with .NET and React.",
                    Status = ApplicationStatus.Applied,
                    AppliedAt = now.AddDays(-1),
                    CreatedAt = now.AddDays(-1),
                    UpdatedAt = now.AddDays(-1)
                });

                await db.SaveChangesAsync();
            }
        }

        var categoryCounts = await db.Jobs
            .Where(j => j.CategoryId != null)
            .GroupBy(j => j.CategoryId)
            .Select(g => new { CategoryId = g.Key!.Value, Count = g.Count() })
            .ToListAsync();

        foreach (var category in await db.Categories.ToListAsync())
        {
            category.JobCount = categoryCounts.FirstOrDefault(x => x.CategoryId == category.Id)?.Count ?? 0;
        }

        await db.SaveChangesAsync();
    }

    private sealed record CompanySeed(
        string Email,
        string Password,
        string FirstName,
        string LastName,
        string CompanyName,
        string Description,
        string? LogoUrl,
        string? Website,
        string? LinkedInUrl,
        string? Industry,
        string? CompanySize,
        string? Location,
        int? FoundedYear);

    private sealed record JobSeed(
        string Title,
        string CompanyName,
        string CategorySlug,
        string Description,
        string Requirements,
        string Benefits,
        EmploymentType EmploymentType,
        ExperienceLevel ExperienceLevel,
        string? Location,
        RemoteType RemoteType,
        decimal? SalaryMin,
        decimal? SalaryMax,
        string Currency,
        IReadOnlyList<string> SkillNames,
        bool IsFeatured,
        int DaysAgo,
        int ViewCount,
        int ApplicationCount);
}
