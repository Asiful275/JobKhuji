
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

        // Job Seeker Profile
        public DbSet<JobSeekerProfile> JobSeekerProfiles
            => Set<JobSeekerProfile>();

        // Employer Profile
        public DbSet<EmployerProfile> EmployerProfiles
            => Set<EmployerProfile>();

        // Job Categories
        public DbSet<Category> Categories
            => Set<Category>();

        // Job Posts
        public DbSet<Job> Jobs
            => Set<Job>();

        // Uploaded CVs
        public DbSet<Cv> Cvs
            => Set<Cv>();

        // Job Applications
        public DbSet<JobApplication> Applications
            => Set<JobApplication>();

        // AI Matching Scores
        public DbSet<MatchScore> MatchScores
            => Set<MatchScore>();

        // Saved Jobs
        public DbSet<SavedJob> SavedJobs
            => Set<SavedJob>();

        // Notifications
        public DbSet<Notification> Notifications
            => Set<Notification>();

        // Job Reports
        public DbSet<JobReport> JobReports
            => Set<JobReport>();
        protected override void OnModelCreating(
    ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            // One Job Seeker -> Many CVs
            modelBuilder.Entity<Cv>()
                .HasOne(c => c.JobSeekerProfile)
                .WithMany()
                .HasForeignKey(c => c.JobSeekerProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            // Job Application Relationships
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

            // AI Matching Relationships
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

            // One Match Score per Job and CV
            modelBuilder.Entity<MatchScore>()
                .HasIndex(m => new
                {
                    m.JobId,
                    m.CvId
                })
                .IsUnique();

            // Saved Job Relationships
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

            // Prevent Saving Same Job Twice
            modelBuilder.Entity<SavedJob>()
                .HasIndex(s => new
                {
                    s.JobSeekerProfileId,
                    s.JobId
                })
                .IsUnique();

            // User Notifications
            modelBuilder.Entity<Notification>()
                .HasOne(n => n.User)
                .WithMany()
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Job Reports
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

            // Ensure Unique Category Names
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Name)
                .IsUnique();
        }
    }
    
}
