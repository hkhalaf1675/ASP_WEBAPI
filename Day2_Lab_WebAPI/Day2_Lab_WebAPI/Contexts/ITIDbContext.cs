using Day2_Lab_WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Day2_Lab_WebAPI.Contexts
{
    public class ITIDbContext:DbContext
    {
        public ITIDbContext()
        {}
        public ITIDbContext(DbContextOptions options):base(options)
        {}

        public DbSet<Department> Departments { get; set; }
        public DbSet<Student> Students { get; set; }
    }
}
