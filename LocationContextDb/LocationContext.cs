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

            var usa = new CountryModel { Id = 1, Name = "USA" };
            var canada = new CountryModel { Id = 2, Name = "Canada" };

            modelBuilder.Entity<CountryModel>().HasData(usa, canada);

            modelBuilder.Entity<ProvinceModel>().HasData(
                new ProvinceModel { Id = 1, CountryId = 1, Name = "California" },
                new ProvinceModel { Id = 2, CountryId = 1, Name = "Texas" },
                new ProvinceModel { Id = 3, CountryId = 1, Name = "Florida" },
                new ProvinceModel { Id = 4, CountryId = 2, Name = "Ontario" },
                new ProvinceModel { Id = 5, CountryId = 2, Name = "Quebec" },
                new ProvinceModel { Id = 6, CountryId = 2, Name = "Alberta" }
            );
        }
    }
}
