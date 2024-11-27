using System.Collections.Generic;
using System.Threading.Tasks;

namespace HvoyaApplication.Models.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<AppUser>> GetAllUsersAsync();
        Task<AppUser?> GetUserByIdAsync(string userId);
    }
}
