using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RiskAware.Server.Models;

namespace RiskAware.Server.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
    {
        public DbSet<Vehicle> Vehicles { get; set; } // TODO -> smazat
        public DbSet<SystemRole> SystemRoles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<RiskProject> RiskProjects { get; set; }
        public DbSet<RiskHistory> RiskHistory { get; set; }
        public DbSet<RiskCategory> RiskCategories { get; set; }
        public DbSet<ProjectPhase> ProjectPhases { get; set; }
        public DbSet<ProjectRole> ProjectRoles { get; set; }
        public DbSet<Risk> Risks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // This ensures that "Users" table will be created.
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
            });

            // Defining composite primary key for ProjectRole
            modelBuilder.Entity<ProjectRole>()
                .HasKey(pr => new { pr.UserId, pr.RiskProjectId });

            // This ensures that SystemRole cannot be deleted while there are users having this role.
            modelBuilder.Entity<User>()
                .HasOne(r => r.SystemRole)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.SystemRoleId)
                .OnDelete(DeleteBehavior.Restrict);

            // This ensures that User cannot be deleted while there are projects created by him.
            // Basically means that admin cannot be deleted.
            modelBuilder.Entity<RiskProject>()
                .HasOne(u => u.User)
                .WithMany(p => p.RiskProjects)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // This ensures that User cannot be deleted while he has a role on a project.
            modelBuilder.Entity<ProjectRole>()
                .HasOne(u => u.User)
                .WithMany(p => p.ProjectRoles)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // This ensures that User cannot be deleted while he has a risks created by him.
            modelBuilder.Entity<Risk>()
                .HasOne(u => u.User)
                .WithMany(r => r.Risks)
                .HasForeignKey(r =>  r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // This ensures that Project cannot be deleted while there are risks associated to it.
            modelBuilder.Entity<Risk>()
                .HasOne(p => p.RiskProject)
                .WithMany(r => r.Risks)
                .HasForeignKey(r => r.RiskProjectId)
                .OnDelete(DeleteBehavior.Restrict);

            // This ensures that User cannot be deleted while he is contributor to history of some risks.
            modelBuilder.Entity<RiskHistory>()
                .HasOne(u => u.User)
                .WithMany(h => h.RiskHistory)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // This ensures that User cannot be deleted while he has some comments.
            modelBuilder.Entity<Comment>()
                .HasOne(u => u.User)
                .WithMany(h => h.Comments)
                .HasForeignKey (h => h.UserId)
                .OnDelete (DeleteBehavior.Restrict);

            // TODO -> az bude namapovana phase na role, tak zajistit aby se po smazani role nemazala faze a naopak -> pouze pokud faze bude mit vic clenu nez jednoho
        }
    }
}
