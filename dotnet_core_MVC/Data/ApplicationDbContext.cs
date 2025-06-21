using dotnet_core_MVC.Models;
using Microsoft.EntityFrameworkCore;

namespace dotnet_core_MVC.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<OtpRecord> OtpRecords { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<UserMessage> UserMessages { get; set; }
        public DbSet<EmployeeCourse> EmployeeCourses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OtpRecord>().ToTable("OtpRecords"); // Optional: Custom table name
        }
    }
    public class User // Sample Model
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
