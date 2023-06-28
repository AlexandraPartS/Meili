using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
        : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<UserDao> Users { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDao>().HasData(
                    new UserDao { Id = 1, NickName = "Tom", PhoneNumber = "1111" },
                    new UserDao { Id = 2, NickName = "Bob", PhoneNumber = "2222" },
                    new UserDao { Id = 3, NickName = "Sam", PhoneNumber = "3333" }
            );
        }

        //public DbSet<backend.Models.UserDto> UserDto { get; set; } = default!;
    }
}
