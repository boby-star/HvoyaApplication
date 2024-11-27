using HvoyaApplication.Data;
using HvoyaApplication.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HvoyaApplication.Models.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddReviewAsync(Review review)
        {
            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Review>> GetReviewsByDessertIdAsync(int dessertId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.DessertId == dessertId)
                .ToListAsync();
        }
    }
}
