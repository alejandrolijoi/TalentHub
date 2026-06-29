using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Serilog;
using TalentHub.Application.Services;
using TalentHub.Application.Validators;
using TalentHub.Infrastructure;
using TalentHub.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IJobService, JobService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<ICandidateService, CandidateService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"] ?? ""))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        var origins = new List<string> { "http://localhost:3000" };
        var frontendUrl = builder.Configuration["Frontend:Url"];
        if (!string.IsNullOrEmpty(frontendUrl))
        {
            origins.AddRange(frontendUrl.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }
        policy.WithOrigins(origins.ToArray())
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TalentHub API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

string dbStatus = "not_checked";
string dbError = "";

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TalentHubDbContext>();
    try
    {
        Log.Information("Attempting to create/migrate database...");
        Console.WriteLine("Attempting to create/migrate database...");
        var created = await db.Database.EnsureCreatedAsync();
        Log.Information("EnsureCreated returned: {Created}", created);
        Console.WriteLine($"EnsureCreated returned: {created}");
        dbStatus = created ? "tables_created" : "tables_already_existed";
    }
    catch (Exception ex)
    {
        dbStatus = "ensure_failed";
        dbError = ex.Message + " | " + (ex.InnerException?.Message ?? "");
        Log.Error(ex, "EnsureCreated failed");
        Console.WriteLine($"DB ENSURE ERROR: {ex.Message}");
        Console.WriteLine($"DB ENSURE INNER: {ex.InnerException?.Message}");
    }

    try
    {
        await DataSeeder.SeedAsync(db);
        dbStatus = "initialized_and_seeded";
        Log.Information("Database seeded successfully");
        Console.WriteLine("Database seeded successfully");
    }
    catch (Exception ex)
    {
        dbStatus = "seed_failed";
        dbError = ex.Message + " | " + (ex.InnerException?.Message ?? "");
        Log.Error(ex, "Database seeding failed");
        Console.WriteLine($"DB SEED ERROR: {ex.Message}");
        Console.WriteLine($"DB SEED INNER: {ex.InnerException?.Message}");
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.MapScalarApiReference();

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/health", async (TalentHubDbContext db) =>
{
    try
    {
        await db.Database.CanConnectAsync();
        return Results.Ok(new { status = "healthy", dbStatus, timestamp = DateTime.UtcNow });
    }
    catch
    {
        return Results.StatusCode(503);
    }
});

app.MapGet("/debug/db", async (TalentHubDbContext db) =>
{
    try
    {
        var canConnect = await db.Database.CanConnectAsync();
        var entityCount = db.Model.GetEntityTypes().Count();

        List<string> tableNames = new();
        var conn = db.Database.GetDbConnection();
        if (conn.State != System.Data.ConnectionState.Open)
            await conn.OpenAsync();
        using (var cmd = conn.CreateCommand())
        {
            cmd.CommandText = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name";
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                tableNames.Add(reader.GetString(0));
        }

        return Results.Ok(new { canConnect, entityCount, dbStatus, dbError, tables = tableNames });
    }
    catch (Exception ex)
    {
        return Results.Ok(new { error = ex.Message, inner = ex.InnerException?.Message, dbStatus, dbError });
    }
});

app.Run();
