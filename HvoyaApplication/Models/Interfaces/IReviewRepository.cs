using System.Collections.Generic;
using System.Threading.Tasks;

namespace HvoyaApplication.Models.Interfaces
{
    public interface IReviewRepository
    {
        Task AddReviewAsync(Review review);
        Task<IEnumerable<Review>> GetReviewsByDessertIdAsync(int dessertId);
    }
}
