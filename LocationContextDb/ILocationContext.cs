using LocationContextDb.Models;
using Microsoft.EntityFrameworkCore;

namespace LocationContextDb
{
    public interface ILocationContext
    {
        DbSet<CountryModel> Countries { get; set; }
        DbSet<ProvinceModel> Provinces { get; set; }
        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}
