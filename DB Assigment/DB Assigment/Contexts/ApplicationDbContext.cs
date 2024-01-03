using DB_Assigment.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DB_Assigment.Contexts
{
    public class ApplicationDbContext:IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions options):base(options)
        {}
        protected override void OnModelCreating(ModelBuilder builder)
        {
            // make the code primary key of the product table
            builder.Entity<Product>().HasKey(P => P.Code);

            base.OnModelCreating(builder);
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
