using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserDao>()
                .HasOne(u => u.PhoneNumber)
                .WithOne(p => p.UserDao)
                .HasForeignKey<UserPhone>(p => p.Id);

            modelBuilder.Entity<UserDao>().ToTable("Users");
            modelBuilder.Entity<UserPhone>().ToTable("Users");
        }
    }
}
