using LocationContextDb.Models;
using Microsoft.EntityFrameworkCore;

namespace LocationContextDb
{
    public class LocationContext : DbContext, ILocationContext
    {
        public DbSet<CountryModel> Countries { get; set; }
        public DbSet<ProvinceModel> Provinces { get; set; }
        public LocationContext(DbContextOptions<LocationContext> options) : base(options)
        {
        }

        public async Task<int> SaveAsync(CancellationToken cancellationToken)
        {
            return await base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CountryModel>().ToTable("countries", "loc");

            modelBuilder.Entity<CountryModel>()
                .HasKey(c => c.Id);

            modelBuilder.Entity<CountryModel>()
                .HasMany(c => c.Provinces)
                .WithOne(p => p.Country)
                .HasForeignKey(p => p.CountryId);

            modelBuilder.Entity<ProvinceModel>().ToTable("provinces", "loc");

            modelBuilder.Entity<ProvinceModel>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<ProvinceModel>()
                .HasOne(p => p.Country)
                .WithMany(c => c.Provinces)
                .HasForeignKey(p => p.CountryId);
        }
    }
}
