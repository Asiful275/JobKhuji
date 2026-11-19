
using JobKhuji.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JobKhuji.DataAccess
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        // Constructor
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ==============================
        // DATABASE TABLES
        // ==============================

        public DbSet<JobSeekerProfile> JobSeekerProfiles
            => Set<JobSeekerProfile>();

        public DbSet<EmployerProfile> EmployerProfiles
            => Set<EmployerProfile>();

        public DbSet<Category> Categories
            => Set<Category>();

        public DbSet<Job> Jobs
            => Set<Job>();

        public DbSet<Cv> Cvs
            => Set<Cv>();

        public DbSet<JobApplication> Applications
            => Set<JobApplication>();

        public DbSet<MatchScore> MatchScores
            => Set<MatchScore>();

        public DbSet<SavedJob> SavedJobs
            => Set<SavedJob>();

        public DbSet<Notification> Notifications
            => Set<Notification>();

        public DbSet<JobReport> JobReports
            => Set<JobReport>();

        // ==============================
        // DATABASE RELATIONSHIPS
        // ==============================

        protected override void OnModelCreating(
            ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --------------------------
            // USER PROFILES
            // --------------------------

            // One User -> One Job Seeker Profile
            modelBuilder.Entity<JobSeekerProfile>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<JobSeekerProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // One User -> One Employer Profile
            modelBuilder.Entity<EmployerProfile>()
                .HasOne(p => p.User)
                .WithOne()
                .HasForeignKey<EmployerProfile>(p => p.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // --------------------------
            // JOB RELATIONSHIPS
            // --------------------------

            // One Employer -> Many Jobs
            modelBuilder.Entity<Job>()
                .HasOne(j => j.EmployerProfile)
                .WithMany()
                .HasForeignKey(j => j.EmployerProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            // One Category -> Many Jobs
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Category)
                .WithMany()
                .HasForeignKey(j => j.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            // --------------------------
            // SALARY PRECISION
            // --------------------------

            modelBuilder.Entity<Job>()
                .Property(j => j.MinimumSalary)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Job>()
                .Property(j => j.MaximumSalary)
                .HasPrecision(18, 2);

            // --------------------------
            // CV RELATIONSHIP
            // --------------------------

            // One Job Seeker -> Many CVs
            modelBuilder.Entity<Cv>()
                .HasOne(c => c.JobSeekerProfile)
                .WithMany()
                .HasForeignKey(c => c.JobSeekerProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            // --------------------------
            // JOB APPLICATIONS
            // --------------------------

            modelBuilder.Entity<JobApplication>()
                .HasOne(a => a.Job)
                .WithMany()
                .HasForeignKey(a => a.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobApplication>()
                .HasOne(a => a.JobSeekerProfile)
                .WithMany()
                .HasForeignKey(a => a.JobSeekerProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobApplication>()
                .HasOne(a => a.Cv)
                .WithMany()
                .HasForeignKey(a => a.CvId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent Duplicate Applications
            modelBuilder.Entity<JobApplication>()
                .HasIndex(a => new
                {
                    a.JobId,
                    a.JobSeekerProfileId
                })
                .IsUnique();

            // --------------------------
            // AI MATCH SCORES
            // --------------------------

            modelBuilder.Entity<MatchScore>()
                .HasOne(m => m.Job)
                .WithMany()
                .HasForeignKey(m => m.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MatchScore>()
                .HasOne(m => m.Cv)
                .WithMany()
                .HasForeignKey(m => m.CvId)
                .OnDelete(DeleteBehavior.Restrict);

            // One Score per Job and CV Pair
            modelBuilder.Entity<MatchScore>()
                .HasIndex(m => new
                {
                    m.JobId,
                    m.CvId
                })
                .IsUnique();

            // --------------------------
            // SAVED JOBS
            // --------------------------

            modelBuilder.Entity<SavedJob>()
                .HasOne(s => s.Job)
                .WithMany()
                .HasForeignKey(s => s.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<SavedJob>()
                .HasOne(s => s.JobSeekerProfile)
                .WithMany()
                .HasForeignKey(s => s.JobSeekerProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            // Prevent Duplicate Saved Jobs
            modelBuilder.Entity<SavedJob>()
                .HasIndex(s => new
                {
                    s.JobSeekerProfileId,
                    s.JobId
                })
                .IsUnique();

            // --------------------------
            // NOTIFICATIONS
            // --------------------------

            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // --------------------------
            // JOB REPORTS
            // --------------------------

            modelBuilder.Entity<JobReport>()
                .HasOne(r => r.Job)
                .WithMany()
                .HasForeignKey(r => r.JobId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<JobReport>()
                .HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // --------------------------
            // CATEGORY CONSTRAINT
            // --------------------------

            // Prevent Duplicate Category Names
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }
    }
}
