using HvoyaApplication.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HvoyaApplication.Models
{
    public class ShoppingCart
    {
        private readonly ApplicationDbContext _context;

        public ShoppingCart(ApplicationDbContext context)
        {
            _context = context;
        }

        public int ShoppingCartId { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ShoppingCartItem> ShoppingCartItems { get; set; } = new List<ShoppingCartItem>();

        // отримання активного кошика для користувача
        public ShoppingCart GetActiveCart(string userId)
        {
            var shoppingCart = _context.ShoppingCarts
                .Include(c => c.ShoppingCartItems)
                .ThenInclude(i => i.Dessert)
                .FirstOrDefault(c => c.UserId == userId && c.IsActive);

            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCart(_context)
                {
                    UserId = userId,
                    IsActive = true
                };

                _context.ShoppingCarts.Add(shoppingCart);
                _context.SaveChanges();
            }

            return shoppingCart;
        }

        // додавання товару в кошик
        public void AddToCart(string userId, Dessert dessert, int quantity = 1)
        {
            var cart = GetActiveCart(userId);

            var cartItem = _context.ShoppingCartItems
                .FirstOrDefault(i => i.ShoppingCartId == cart.ShoppingCartId && i.DessertId == dessert.DessertId);

            if (cartItem == null)
            {
                cartItem = new ShoppingCartItem
                {
                    ShoppingCartId = cart.ShoppingCartId,
                    DessertId = dessert.DessertId,
                    Quantity = quantity
                };

                _context.ShoppingCartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }

            _context.SaveChanges();
        }

        // закриття кошика та створення замовлення
        public void Checkout(string userId)
        {
            var cart = GetActiveCart(userId);

            if (cart.ShoppingCartItems.Any())
            {
                // створення замовлення
                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    OrderTotal = cart.ShoppingCartItems.Sum(i => i.Dessert.Price * i.Quantity),
                    OrderItems = cart.ShoppingCartItems.Select(i => new OrderItem
                    {
                        DessertId = i.DessertId,
                        Quantity = i.Quantity,
                        Price = i.Dessert.Price
                    }).ToList()
                };

                _context.Orders.Add(order);

                // закриття кошика
                cart.IsActive = false;

                // створення нового порожнього кошика
                var newCart = new ShoppingCart(_context)
                {
                    UserId = userId,
                    IsActive = true
                };

                _context.ShoppingCarts.Add(newCart);
                _context.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Кошик порожній. Замовлення не може бути створене.");
            }
        }
    }
}
