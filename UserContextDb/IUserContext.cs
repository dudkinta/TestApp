using Microsoft.EntityFrameworkCore;
using UserContextDb.Models;

namespace UserContextDb
{
    public interface IUserContext
    {
        DbSet<UserModel> Users { get; }
        Task<int> SaveAsync(CancellationToken cancellationToken);
    }
}
