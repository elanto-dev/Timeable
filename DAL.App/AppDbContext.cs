using System;
using Domain;
using Domain.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.App
{
    public class AppDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public DbSet<AppUser> AppUsers { get; set; } = default!;
        public DbSet<AppUsersScreen> AppUsersScreens { get; set; } = default!;
        public DbSet<Event> Events { get; set; } = default!;
        public DbSet<EventInSchedule> EventInSchedules { get; set; } = default!;
        public DbSet<Picture> Pictures { get; set; } = default!;
        public DbSet<PictureInScreen> PictureInScreens { get; set; } = default!;
        public DbSet<Schedule> Schedules { get; set; } = default!;
        public DbSet<ScheduleInScreen> ScheduleInScreens { get; set; } = default!;
        public DbSet<Screen> Screens { get; set; } = default!;
        public DbSet<Subject> Subjects { get; set; } = default!;
        public DbSet<SubjectInSchedule> SubjectInSchedules { get; set; } = default!;
        public DbSet<Teacher> Teachers { get; set; } = default!;
        public DbSet<TeacherInSubjectEvent> TeacherInSubjectEvents { get; set; } = default!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SubjectInSchedule>()
                .HasIndex(p => p.UniqueIdentifier)
                .IsUnique();
            modelBuilder.Entity<Screen>()
                .HasIndex(s => s.UniqueIdentifier)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
