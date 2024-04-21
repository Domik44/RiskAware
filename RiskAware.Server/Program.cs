using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Configurations;
using RiskAware.Server.Data;
using RiskAware.Server.Models;
using RiskAware.Server.Queries;
using RiskAware.Server.Seeds;
using System.Globalization;

namespace RiskAware.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Set default czech datetime format
            Thread.CurrentThread.CurrentCulture = new CultureInfo("cs-CZ");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<RiskProjectQueries>();
            builder.Services.AddScoped<ProjectPhaseQueries>();
            builder.Services.AddScoped<RiskQueries>();
            builder.Services.AddScoped<ProjectRoleQueries>();

            // MSSQL database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // MSSQL database
            //builder.Services.AddDbContext<AppDbContext>(options =>
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("TestConnection")));

            // User authentication
            builder.Services.AddIdentity<User, IdentityRole>(IdentityConfiguration.ConfigureIdentityOptions)
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>();

            // Cookies settings
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.ExpireTimeSpan = TimeSpan.FromDays(2);
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // Setting always breaks tests
            });

            WebApplication app = builder.Build();

            if (args.Length > 0)
            {
                if (args[0] == "seed")
                {
                    // For seed data use cmd: dotnet run seed
                    await DbSeeder.SeedAll(app.Services);
                    return;
                }
                else if (args[0] == "unseed")
                {
                    // For database table cleanup use cmd: dotnet run unseed
                    using IServiceScope scope = app.Services.CreateScope();
                    AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    DbCleaner.TruncateAllTablesData(dbContext);
                    return;
                }
                else if (args[0] == "delete-db")
                {
                    // For database delete use cmd: dotnet run delete-db
                    using IServiceScope scope = app.Services.CreateScope();
                    AppDbContext dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    DbCleaner.DeleteEntireDb(dbContext);
                    return;
                }
            }

            // Set Cors
            app.UseCors(corsBuilder =>
            {
                corsBuilder.WithOrigins(
                        "https://localhost:5173",
                        "http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
            app.UseCors("AllowSpecificOrigin");


            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
