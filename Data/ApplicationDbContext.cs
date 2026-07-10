using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Models;

namespace WebApplication5.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Teacher> teachers { get; set; }
        public DbSet<Lecture> Lectures { get; set; }
        public DbSet<Teacher_Lecture> teacher_Lectures { get; set; }
        public DbSet<Dayes> dayes { get; set; }
        public DbSet<ScheduleHour> ScheduleHours { get; set; }
        public DbSet<Rooms> rooms { get; set; }
        public DbSet<Stages> stages { get; set; }
        public DbSet<FinalSchedule> finalSchedules { get; set; }
    }
}
