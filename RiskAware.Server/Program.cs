using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Configurations;
using RiskAware.Server.Data;
using RiskAware.Server.Models;
using RiskAware.Server.Seeds;

namespace RiskAware.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add in-memory database
            //builder.Services.AddDbContext<AppDbContext>(options => 
            //options.UseInMemoryDatabase("RiskAwareDb"));

            // MSSQL database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // User authentication
            builder.Services.AddIdentity<User, IdentityRole>(IdentityConfiguration.ConfigureIdentityOptions)
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            // Cookies settings
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.ExpireTimeSpan = TimeSpan.FromDays(2);
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            var app = builder.Build();

            // For seed data use cmd: dotnet run seed
            if (args.Length > 0 && args[0] == "seed")
            {
                await DbSeeder.SeedAll(app.Services);
                return;
            }
            // For database cleanup use cmd: dotnet run clear-db
            if (args.Length > 0 && args[0] == "clear-db")
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await DbCleaner.ClearAllData(dbContext, userManager, roleManager);
                return;
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
            app.UseCors("AllowSpecificOrigin");

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
