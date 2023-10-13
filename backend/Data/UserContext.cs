using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace backend.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options)
        : base(options)
        {
        }
        public DbSet<UserDao> Users { get; set; } = null!;
        public DbSet<FileModel> Files { get; set; }

    }
}
