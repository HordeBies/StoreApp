using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Store.Models;

namespace Store.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyProduct> CompanyProducts { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OrderHeader> OrderHeaders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(new Category[]
            {
                new Category { Id = 1, Name = "Action", DisplayOrder = 1 },
                new Category { Id = 2, Name = "SciFi", DisplayOrder = 2 },
                new Category { Id = 3, Name = "History", DisplayOrder = 3 },
            });

            modelBuilder.Entity<Company>().HasData(new Company[]
            {
                new Company{ Id = 1, Name = "Tagtune", StreetAddress= "5 Erie Court", City = "Floriano", PostalCode = "64800-000", PhoneNumber = "362-555-3461"},
                new Company{ Id = 2, Name = "Jabbercube", StreetAddress= "592 Porter Way", City = "Salamina", PostalCode = "477047", PhoneNumber = "725-831-6046"},
                new Company{ Id = 3, Name = "Skynoodle", StreetAddress= "73428 Kipling Junction", City = "Sankoutang", PostalCode = "1237", PhoneNumber = "251-502-3387"},
            });
            modelBuilder.Entity<CompanyProduct>().HasData(new CompanyProduct[]
            {
                new CompanyProduct{CompanyId = 1, ProductId = 2, ListPrice = 50, Price = 45},
                new CompanyProduct{CompanyId = 2, ProductId = 2, ListPrice = 60, Price = 55},
                new CompanyProduct{CompanyId = 3, ProductId = 2, ListPrice = 65, Price = 60},

                new CompanyProduct{CompanyId = 1, ProductId = 1, ListPrice = 100, Price = 90},
                new CompanyProduct{CompanyId = 2, ProductId = 1, ListPrice = 110, Price = 100},
                new CompanyProduct{CompanyId = 3, ProductId = 1, ListPrice = 115, Price = 105},

                new CompanyProduct{CompanyId = 1, ProductId = 3, ListPrice = 70, Price = 60},
                new CompanyProduct{CompanyId = 2, ProductId = 3, ListPrice = 80, Price = 70},
                new CompanyProduct{CompanyId = 3, ProductId = 3, ListPrice = 85, Price = 75},
            });

            modelBuilder.Entity<CompanyProduct>().HasKey(cp => new { cp.CompanyId, cp.ProductId });
            modelBuilder.Entity<Product>() // To use external table for many-to-many relationship and not create a new table for skip navigation
            .HasMany(e => e.Companies)
            .WithMany(e => e.Products)
            .UsingEntity<CompanyProduct>();

            modelBuilder.Entity<Product>().HasData(
                new Product
                {
                    Id = 1,
                    Title = "Fortune of Time",
                    Author = "Billy Spark",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SWD9999001",
                    CategoryID = 1,
                    ImageURL = "",
                },
                new Product
                {
                    Id = 2,
                    Title = "Dark Skies",
                    Author = "Nancy Hoover",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "CAW777777701",
                    CategoryID = 2,
                    ImageURL = "",
                },
                new Product
                {
                    Id = 3,
                    Title = "Vanish in the Sunset",
                    Author = "Julian Button",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "RITO5555501",
                    CategoryID = 3,
                    ImageURL = "",
                },
                new Product
                {
                    Id = 4,
                    Title = "Cotton Candy",
                    Author = "Abby Muscles",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "WS3333333301",
                    CategoryID = 2,
                    ImageURL = "",
                },
                new Product
                {
                    Id = 5,
                    Title = "Rock in the Ocean",
                    Author = "Ron Parker",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "SOTJ1111111101",
                    CategoryID = 3,
                    ImageURL = "",
                },
                new Product
                {
                    Id = 6,
                    Title = "Leaves and Wonders",
                    Author = "Laura Phantom",
                    Description = "Praesent vitae sodales libero. Praesent molestie orci augue, vitae euismod velit sollicitudin ac. Praesent vestibulum facilisis nibh ut ultricies.\r\n\r\nNunc malesuada viverra ipsum sit amet tincidunt. ",
                    ISBN = "FOT000000001",
                    CategoryID = 1,
                    ImageURL = "",
                }
            );

        }
    }
}
