using HvoyaApplication.Data;
using HvoyaApplication.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace HvoyaApplication.Models.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ShoppingCart> GetActiveCartAsync(string userId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.ShoppingCartItems)
                    .ThenInclude(i => i.Dessert)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new ShoppingCart(_context) { UserId = userId };
                _context.ShoppingCarts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task AddToCartAsync(string userId, Dessert dessert, int quantity)
        {
            var cart = await GetActiveCartAsync(userId);

            var cartItem = cart.ShoppingCartItems
                .FirstOrDefault(i => i.DessertId == dessert.DessertId);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new ShoppingCartItem
                {
                    DessertId = dessert.DessertId,
                    Quantity = quantity,
                    ShoppingCartId = cart.ShoppingCartId
                };
                cart.ShoppingCartItems.Add(cartItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(string userId, int dessertId)
        {
            var cart = await GetActiveCartAsync(userId);

            var cartItem = cart.ShoppingCartItems
                .FirstOrDefault(i => i.DessertId == dessertId);

            if (cartItem != null)
            {
                cart.ShoppingCartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await GetActiveCartAsync(userId);

            cart.ShoppingCartItems.Clear();
            await _context.SaveChangesAsync();
        }
    }
}
