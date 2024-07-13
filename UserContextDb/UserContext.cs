using Microsoft.EntityFrameworkCore;
using UserContextDb.Models;

namespace UserContextDb
{
    public class UserContext : DbContext, IUserContext
    {
        public DbSet<UserModel> Users { get; set; }
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>().ToTable("users", "usr");

            modelBuilder.Entity<UserModel>()
                .HasKey(_ => _.Id);
        }
    }
}
