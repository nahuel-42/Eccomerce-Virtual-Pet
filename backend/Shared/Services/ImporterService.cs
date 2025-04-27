using System.Globalization;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Backend.Modules.Products.Domain.Entities;
using Backend.Modules.Products.Infrastructure.Persistence;
using Backend.Modules.Users.Domain.Entities;
using Backend.Modules.Users.Infrastructure.Persistence;
// using Backend.Modules.Orders.Domain.Entities;

public class ImporterService
{
    private readonly UsersDbContext _usersDbContext;
    private readonly ProductDbContext _productsDbContext;
    // private readonly OrderDbContext _orderDbContext;


    // Agregar OrderDBContext cuando se implemente el modulo de orders
    public ImporterService(UsersDbContext usersDbContext, ProductDbContext productsDbContext)
    {
        _usersDbContext = usersDbContext;
        _productsDbContext = productsDbContext;
    }


    public async Task ImportAllAsync()
    {
        await ImportRolesAsync();
        await ImportUsersAsync();
        await ImportProductsAsync();
        await ImportAnimalCategoriesAsync();
        await ImportProductAnimalCategoriesAsync();
        // await ImportOrderStatusesAsync();
    }

    private async Task ImportRolesAsync()
    {
        if (await _usersDbContext.Roles.AnyAsync())
            return;

        var roles = ReadCsv<Role>("csv/roles.csv");
        _usersDbContext.Roles.AddRange(roles);
        await _usersDbContext.SaveChangesAsync();
    }

    private async Task ImportUsersAsync()
    {
        if (await _usersDbContext.Users.AnyAsync())
            return;

        var users = ReadCsv<User>("csv/users.csv");
        _usersDbContext.Users.AddRange(users);
        await _usersDbContext.SaveChangesAsync();
    }

    private async Task ImportProductsAsync()
    {
        if (await _productsDbContext.Products.AnyAsync())
            return;

        var products = ReadCsv<Product>("csv/products.csv");
        _productsDbContext.Products.AddRange(products);
        await _productsDbContext.SaveChangesAsync();
    }

    private async Task ImportAnimalCategoriesAsync()
    {
        if (await _productsDbContext.AnimalCategories.AnyAsync())
            return;

        var categories = ReadCsv<AnimalCategory>("csv/animal_categories.csv");
        _productsDbContext.AnimalCategories.AddRange(categories);
        await _productsDbContext.SaveChangesAsync();
    }

    private async Task ImportProductAnimalCategoriesAsync()
    {
        if (await _productsDbContext.ProductAnimalCategories.AnyAsync())
            return;

        var relations = ReadCsv<ProductAnimalCategory>("csv/product_animal_categories.csv");
        _productsDbContext.ProductAnimalCategories.AddRange(relations);
        await _productsDbContext.SaveChangesAsync();
    }

    // private async Task ImportOrderStatusesAsync()
    // {
    //     if (await _dbContext.OrderStatuses.AnyAsync())
    //         return;

    //     var statuses = ReadCsv<OrderStatus>("csv/order_statuses.csv");
    //     _dbContext.OrderStatuses.AddRange(statuses);
    //     await _dbContext.SaveChangesAsync();
    // }

    private List<T> ReadCsv<T>(string filePath)
    {
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        return csv.GetRecords<T>().ToList();
    }
}
