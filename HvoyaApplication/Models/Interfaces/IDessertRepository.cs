using System.Collections.Generic;
using System.Threading.Tasks;

namespace HvoyaApplication.Models.Interfaces
{
    public interface IDessertRepository
    {
        Task<IEnumerable<Dessert>> GetAllDessertsAsync();
        Task<Dessert?> GetDessertByIdAsync(int id);
        Task AddDessertAsync(Dessert dessert);
        Task UpdateDessertAsync(Dessert dessert);
        Task DeleteDessertAsync(int id);
        Task<IEnumerable<Dessert>> GetDessertsByCategoryAsync(string categoryName);
        Task<IEnumerable<Dessert>> SearchDessertsAsync(string searchTerm);
    }
}
