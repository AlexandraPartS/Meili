using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
        : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<UserDao> Users { get; set; } = null!;
        public DbSet<FileModel> Files { get; set; }
    }
}
