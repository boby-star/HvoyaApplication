using System.Threading.Tasks;

namespace HvoyaApplication.Models.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<ShoppingCart> GetActiveCartAsync(string userId);
        Task AddToCartAsync(string userId, Dessert dessert, int quantity);
        Task RemoveFromCartAsync(string userId, int dessertId);
        Task ClearCartAsync(string userId);
    }
}
