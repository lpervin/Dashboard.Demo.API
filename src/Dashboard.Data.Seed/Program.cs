// See https://aka.ms/new-console-template for more information

//

using Bogus;
using Dashboard.Data.Seed;
using Dashboard.Data.Seed.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

string[] productCategories = new[]
{
    "Electronics",
    "Clothing",
    "Home & Kitchen",
    "Beauty",
    "Sports",
    "Books",
    "Toys",
    "Health & Personal Care",
    "Automotive",
    "Pet Supplies",
    "Jewelry",
    "Tools & Home Improvement",
    "Baby Products",
    "Office Supplies",
    "Musical Instruments",
    "Outdoor & Garden",
    "Groceries",
    "Movies & TV Shows",
    "Video Games",
    "Furniture",
    "Sports & Fitness Equipment",
    "Watches",
    "Party Supplies",
    "Crafts & DIY",
    "Luggage & Travel Accessories"
};


    var configurationBuilder = new ConfigurationBuilder()
        .AddUserSecrets<Program>()
        .Build();

    var connectionString = configurationBuilder.GetConnectionString("eCommerceDb");

    var optionsBuilder = new DbContextOptionsBuilder<ProductsDbContext>();
    optionsBuilder.UseNpgsql(connectionString);

    using (var dbContext = new ProductsDbContext(optionsBuilder.Options))
    {

        if (dbContext.Products.Any())
        {
            Console.WriteLine("Products Already Seeded");
            Console.ReadLine();
            return;
        }

        List<ProductCategory> seedProductCategories = SeedProductCategories(dbContext);
        //var productCategory = dbContext.ProductCategories.ToList();
        foreach (var category in seedProductCategories)
        {
            var faker = new Faker<Product>()
                .RuleFor(p => p.ProductName, (f,p) =>  f.Commerce.ProductName())
                .RuleFor(p => p.Description, (f,p) => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, (f, p) => decimal.Parse(f.Commerce.Price()))
                .RuleFor(p => p.CategoryId , (f,p) => category.Id)
                .RuleFor(p => p.Category , (f,p) => category)
                ;
            var fakeProducts = faker.Generate(1000);
            dbContext.Products.AddRange(fakeProducts);
        }

        dbContext.SaveChanges();

        var prodCount = dbContext.Products.Count();
        Console.WriteLine($"{prodCount} products seeded!");
        Console.ReadLine();

    }

List<ProductCategory> SeedProductCategories(ProductsDbContext dbContext)
{
    if (dbContext.ProductCategories.Any())
        return dbContext.ProductCategories.ToList();
    
    dbContext.ProductCategories.AddRange(
                productCategories.Select(pc => new ProductCategory() {Name = pc, Description = "Description:" + pc}));
        dbContext.SaveChanges();
        return dbContext.ProductCategories.ToList();
}


