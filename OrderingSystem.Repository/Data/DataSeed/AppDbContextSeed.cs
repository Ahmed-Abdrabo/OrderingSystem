using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using OrderingSystem.Core.Entities.Identity;
using OrderingSystem.Core.Entities.Order;
using OrderingSystem.Repository.Data;


namespace OrderingSystem.Repository.Identity.DataSeed
{
    public static class AppDbContextSeed
    {
        public async static Task SeedUserAsync(UserManager<Customer> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            if (_userManager.Users.Count() == 0)
            {
                var user1 = new Customer()
                {
                    DisplayName = "Admin",
                    Email = "admin@gmail.com",
                    UserName = "admin",
                    PhoneNumber = "123456789",
                    Address = new Address
                    {
                        FirstName = "Ahmed",
                        LastName = "Abdrabo",
                        Street = "123 Main St",
                        City = "Cairo",
                        Country = "Egypt"
                    }
                };
                var user2 = new Customer()
                {
                    DisplayName = "John Doe",
                    Email = "johndoe@example.com",
                    UserName = "johndoe",
                    PhoneNumber = "987654321",
                    Address = new Address
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        Street = "456 Elm Street",
                        City = "New York",
                        Country = "USA"
                    }
                };
                var result1 = await _userManager.CreateAsync(user1, "Admin123@");
                var result2 = await _userManager.CreateAsync(user2, "Admin123@");

                if (result1.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user1, "Admin"); 
                }

                if (result2.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user2, "Customer");  
                }
            }
        }

        public async static Task SeedAsync(AppDbContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                if (!context.Products.Any())
                {
                    var products = new List<Product>
                        {
                            new Product { Name = "Laptop", Price = 100.50m, Stock = 180 },
                            new Product { Name = "Smartphone", Price = 200.00m, Stock = 200 },
                            new Product { Name = "Apple iPhone 14", Price = 799.99m, Stock = 250 },
                            new Product { Name = "Samsung Galaxy S23", Price = 899.99m, Stock = 180 },
                            new Product { Name = "Dell XPS 13 Laptop", Price = 1299.99m, Stock = 190 },
                            new Product { Name = "Sony WH-1000XM5 Headphones", Price = 349.99m, Stock = 250 },
                            new Product { Name = "Microsoft Surface Pro 9", Price = 1099.99m, Stock = 220 },
                            new Product { Name = "HP Spectre x360", Price = 1399.99m, Stock = 110 },
                            new Product { Name = "Google Pixel 7", Price = 599.99m, Stock = 175 },
                            new Product { Name = "Bose QuietComfort 45", Price = 299.99m, Stock = 300 },
                            new Product { Name = "iPad Pro 12.9", Price = 1099.99m, Stock = 90 },
                            new Product { Name = "Lenovo ThinkPad X1 Carbon", Price = 1499.99m, Stock = 95 },
                            new Product { Name = "OnePlus 11", Price = 699.99m, Stock = 160 },
                            new Product { Name = "Nintendo Switch OLED", Price = 349.99m, Stock = 220 },
                            new Product { Name = "MacBook Pro 16", Price = 2399.99m, Stock = 70 },
                            new Product { Name = "Samsung Galaxy Tab S8", Price = 799.99m, Stock = 130 },
                            new Product { Name = "Sony PlayStation 5", Price = 499.99m, Stock = 180 },
                            new Product { Name = "Xbox Series X", Price = 499.99m, Stock = 175 },
                            new Product { Name = "DJI Mavic Air 2 Drone", Price = 999.99m, Stock = 50 },
                            new Product { Name = "GoPro Hero 11", Price = 499.99m, Stock = 200 }
                        };

                    // Save to database
                    context.Products.AddRange(products);
                    context.SaveChanges();

                    await context.Products.AddRangeAsync(products);
                    await context.SaveChangesAsync();
                }

                if (!context.Orders.Any())
                {
                    var users = context.Users.ToList();
                    var products = context.Products.ToList();

                    var random = new Random();

                    foreach (var user in users)
                    {
                        var orders = new List<Order>();

                        for (int i = 0; i < 5; i++) // Create 5 orders per user
                        {
                            var selectedProducts = products.OrderBy(x => random.Next()).Take(3).ToList(); // Pick 3 random products

                            var orderItems = selectedProducts.Select(p => new OrderItem
                            {
                                ProductId = p.Id,
                                ProductName = p.Name,
                                Quantity = random.Next(1, 5), // Random quantity between 1-4
                                Price = p.Price
                            }).ToList();

                            var totalAmount = orderItems.Sum(oi => oi.Price * oi.Quantity);

                            orders.Add(new Order
                            {
                                CustomerId = user.Id,
                                OrderDate = DateTime.UtcNow.AddDays(-random.Next(1, 30)), // Random past date within 30 days
                                TotalAmount = totalAmount,
                                OrderItems = orderItems
                            });
                        }

                        context.Orders.AddRange(orders);
                    }

                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger("AppDbContextSeed"); 
                logger.LogError(ex, "Error seeding orders");
            }
        }
    }
}
