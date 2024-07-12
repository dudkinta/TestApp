using LocationContextDb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationContextDb
{
    public interface ILocationContext
    {
        DbSet<CountryModel> Countries { get; set; }
        DbSet<ProvinceModel> Provinces { get; set; }
        Task<int> SaveChangesAsync();
    }
}
