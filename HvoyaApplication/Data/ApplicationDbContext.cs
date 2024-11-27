using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HvoyaApplication.Models;

namespace HvoyaApplication.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Dessert> Desserts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts {  get; set; }
        public DbSet<Review> Reviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ShoppingCart>()
                .HasOne(sc => sc.User)
                .WithMany(u => u.ShoppingCarts)
                .HasForeignKey(sc => sc.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // shoppingCartItem зв’язок із shoppingCart
            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.ShoppingCart)
                .WithMany(sc => sc.ShoppingCartItems)
                .HasForeignKey(sci => sci.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            // shoppingCartItem зв’язок із dessert
            modelBuilder.Entity<ShoppingCartItem>()
                .HasOne(sci => sci.Dessert)
                .WithMany(d => d.ShoppingCartItems)
                .HasForeignKey(sci => sci.DessertId)
                .OnDelete(DeleteBehavior.Cascade);

            // dessert зв’язок із category
            modelBuilder.Entity<Dessert>()
                .HasOne(d => d.Category)
                .WithMany(c => c.Desserts)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            // order зв’язок із AppUser
            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // orderItem зв’язок із order
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // orderItem зв’язок із dessert
            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Dessert)
                .WithMany(d => d.OrderItems)
                .HasForeignKey(oi => oi.DessertId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>().HasData( //наповнюємо категорії 
                new Category { CategoryId = 1, CategoryName = "Торти", Description = "Торти на любий смак і вагу" },
                new Category { CategoryId = 2, CategoryName = "Кекси", Description = "Смачне печиво для вас" });

            modelBuilder.Entity<Dessert>().HasData( //наповнюємо початковими десертами
                new Dessert
                {
                    DessertId = 1,
                    Name = "Шоколадний торт",
                    Description = "Смачний шоколадний торт із кремом",
                    Price = 250,
                    ImageUrl = "/img/cacao-tortyk.jpg",
                    IsAvailable = true,
                    CategoryId = 1
                },
                new Dessert
                {
                    DessertId = 2,
                    Name = "Ванільний кекс",
                    Description = "Ніжний кекс із ванільним смаком",
                    Price = 50,
                    ImageUrl = "/img/keksi_vanyl.jpg",
                    IsAvailable = true,
                    CategoryId = 2
                },
                new Dessert
                {
                    DessertId = 3,
                    Name = "Чізкейк",
                    Description = "Класичний сирний чізкейк",
                    Price = 200,
                    ImageUrl = "/img/chees_cake.jpg",
                    IsAvailable = true,
                    CategoryId = 1
                }
            );

            modelBuilder.Entity<Order>()
                 .Property(o => o.OrderTotal)
                 .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<OrderItem>()
                 .Property(oi => oi.Price)
                 .HasColumnType("decimal(18,2)");
                       
        }
    }

    
}
