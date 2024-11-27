using HvoyaApplication.Data;
using HvoyaApplication.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HvoyaApplication.Models.Repositories
{
    public class DessertRepository : IDessertRepository
    {
        private readonly ApplicationDbContext _context;

        public DessertRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Dessert>> GetAllDessertsAsync()
        {
            return await _context.Desserts
                .Include(d => d.Category)
                .ToListAsync();
        }


        public async Task<Dessert?> GetDessertByIdAsync(int id)
        {
            return await _context.Desserts
                .Include(d => d.Category)
                .FirstOrDefaultAsync(d => d.DessertId == id);
        }

        public async Task AddDessertAsync(Dessert dessert)
        {
            try
            {
                _context.Desserts.Add(dessert);
                Console.WriteLine($"Name: {dessert.Name}, Price: {dessert.Price}, CategoryId: {dessert.CategoryId}");
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка збереження: {ex.Message}");
            }

        }

        public async Task UpdateDessertAsync(Dessert dessert)
        {
            _context.Desserts.Update(dessert);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDessertAsync(int id)
        {
            var dessert = await _context.Desserts.FirstOrDefaultAsync(d => d.DessertId == id);
            if (dessert != null)
            {
                _context.Desserts.Remove(dessert);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Dessert>> GetDessertsByCategoryAsync(string categoryName)
        {
            return await _context.Desserts
                .Include(d => d.Category)
                .Where(d => d.Category.CategoryName.Equals(categoryName))
                .ToListAsync();
        }

        public async Task<IEnumerable<Dessert>> SearchDessertsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Enumerable.Empty<Dessert>();

            return await _context.Desserts
                .Include(d => d.Category)
                .Where(d => d.Name.Contains(searchTerm))
                .ToListAsync();
        }
    }
}
