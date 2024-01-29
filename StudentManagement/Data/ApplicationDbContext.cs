using Microsoft.EntityFrameworkCore;
using StudentManagement.Models.Entities;

namespace StudentManagement.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Student> Students { get; set; }
        public DbSet<Scores> Scores { get; set; }
        public DbSet<Subject> Subjects { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Nếu chưa có dữ liệu thì thêm dữ liệu mẫu
            modelBuilder.Entity<Student>().HasData(
                              new Student
                              {
                                  Id = System.Guid.NewGuid(),
                                  Username = "admin",
                                  Password = "admin",
                                  Name = "Vĩnh Trung",
                                  Phone = "0123456789",
                                  Email = "admin@gmai.com",
                                  Role = "Admin",


                              });
        }

    }
}
