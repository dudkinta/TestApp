using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserContextDb.Models;

namespace UserContextDb
{
    public interface IUserContext
    {
        DbSet<UserModel> Users { get; }
        Task<int> SaveChangesAsync();
    }
}
