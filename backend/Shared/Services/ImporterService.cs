using System.Globalization;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.EntityFrameworkCore;
using Backend.Modules.Products.Domain.Entities;
using Backend.Modules.Products.Infrastructure.Persistence;
using Backend.Modules.Users.Domain.Entities;
using Backend.Modules.Users.Infrastructure.Persistence;
using Backend.Modules.Orders.Domain.Entities;
using Backend.Modules.Orders.Infrastructure.Persistence;
using CsvHelper.Configuration.Attributes;

public class ImporterService
{
    private readonly UsersDbContext _usersDbContext;
    private readonly ProductsDbContext _productsDbContext;
    private readonly OrdersDbContext _ordersDbContext;

    public ImporterService(UsersDbContext usersDbContext, ProductsDbContext productsDbContext, OrdersDbContext ordersDbContext)
    {
        _usersDbContext = usersDbContext;
        _productsDbContext = productsDbContext;
        _ordersDbContext = ordersDbContext;
    }

    public async Task ImportAllAsync()
    {
        await ResetDatabaseAsync();
        await ImportRolesAsync();
        await ImportProductsAsync();
        await ImportAnimalCategoriesAsync();
        await ImportProductAnimalCategoriesAsync();
        await ImportOrderStatusesAsync();
    }

    private async Task ImportRolesAsync()
    {
        if (await _usersDbContext.Roles.AnyAsync())
        {
            Console.WriteLine("Roles already exist, skipping import.");
            return;
        }

        var roles = ReadCsv<Role>("csv/roles.csv");
        _usersDbContext.Roles.AddRange(roles);
        await _usersDbContext.SaveChangesAsync();
        Console.WriteLine("Roles imported successfully.");
    }

    private async Task ImportProductsAsync()
    {
        if (await _productsDbContext.Products.AnyAsync())
        {
            Console.WriteLine("Products already exist, skipping import.");
            return;
        }

        var products = ReadCsv<Product>("csv/products.csv");
        _productsDbContext.Products.AddRange(products);
        await _productsDbContext.SaveChangesAsync();
        Console.WriteLine("Products imported successfully.");
    }

    private async Task ImportAnimalCategoriesAsync()
    {
        var existingCategories = await _productsDbContext.AnimalCategories.ToListAsync();
        if (existingCategories.Any())
        {
            Console.WriteLine($"Animal categories already exist ({existingCategories.Count} records):");
            foreach (var category in existingCategories)
            {
                Console.WriteLine($" - Id: {category.Id}, Name: {category.Name ?? "NULL"}");
            }
            Console.WriteLine("Clearing existing animal categories...");
            _productsDbContext.AnimalCategories.RemoveRange(existingCategories);
            await _productsDbContext.SaveChangesAsync();
        }

        var categories = ReadCsv<AnimalCategory>("csv/animal_categories.csv");
        var validCategories = categories
            .Where(c => !string.IsNullOrWhiteSpace(c.Name))
            .ToList();

        if (validCategories.Count == 0)
        {
            Console.WriteLine("No valid animal categories found in CSV.");
            return;
        }

        Console.WriteLine($"Importing {validCategories.Count} animal categories:");
        foreach (var category in validCategories)
        {
            Console.WriteLine($" - Name: {category.Name}");
        }

        _productsDbContext.AnimalCategories.AddRange(validCategories);
        await _productsDbContext.SaveChangesAsync();
        Console.WriteLine("Animal categories imported successfully.");
    }

    private async Task ImportOrderStatusesAsync()
    {
        if (await _ordersDbContext.OrderStatuses.AnyAsync())
        {
            Console.WriteLine("Statuses already exist, skipping import.");
            return;
        }

        var statuses = ReadCsv<OrderStatus>("csv/order_statuses.csv");
        _ordersDbContext.OrderStatuses.AddRange(statuses);
        await _ordersDbContext.SaveChangesAsync();
        Console.WriteLine("Statuses imported successfully.");
    }

    private async Task ImportProductAnimalCategoriesAsync()
    {
        if (await _productsDbContext.ProductAnimalCategories.AnyAsync())
        {
            Console.WriteLine("Product-animal categories already exist, skipping import.");
            return;
        }

        var existingCategories = await _productsDbContext.AnimalCategories.ToListAsync();
        Console.WriteLine($"Found {existingCategories.Count} animal categories in database:");
        foreach (var category in existingCategories)
        {
            Console.WriteLine($" - Id: {category.Id}, Name: {category.Name ?? "NULL"}");
        }

        var relations = ReadCsv<ProductAnimalCategory>("csv/product_animal_categories.csv");
        var validRelations = new List<ProductAnimalCategory>();

        foreach (var relation in relations)
        {
            var productExists = await _productsDbContext.Products.AnyAsync(p => p.Id == relation.ProductId);
            var categoryExists = await _productsDbContext.AnimalCategories.AnyAsync(c => c.Id == relation.AnimalCategoryId);

            if (productExists && categoryExists)
            {
                validRelations.Add(relation);
            }
            else
            {
                Console.WriteLine($"Invalid relation: ProductId={relation.ProductId}, AnimalCategoryId={relation.AnimalCategoryId}");
            }
        }

        if (validRelations.Count == 0)
        {
            Console.WriteLine("No valid product-animal category relations to import.");
            return;
        }

        Console.WriteLine($"Importing {validRelations.Count} product-animal category relations...");
        _productsDbContext.ProductAnimalCategories.AddRange(validRelations);
        await _productsDbContext.SaveChangesAsync();
        Console.WriteLine("Product-animal category relations imported successfully.");
    }

    private List<T> ReadCsv<T>(string filePath)
    {
        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            PrepareHeaderForMatch = args => args.Header.ToLower(),
            HeaderValidated = null,
            MissingFieldFound = null
        };

        using var reader = new StreamReader(filePath, Encoding.UTF8);
        using var csvReader = new CsvReader(reader, config);

        if (typeof(T) == typeof(ProductAnimalCategory))
        {
            csvReader.Context.RegisterClassMap<ProductAnimalCategoryMap>();
        }

        csvReader.Read();
        csvReader.ReadHeader();
        var records = csvReader.GetRecords<T>().ToList();

        Console.WriteLine($"Read {records.Count} records from {filePath}:");
        foreach (var record in records)
        {
            if (record is AnimalCategory animalCategory)
            {
                Console.WriteLine($" - AnimalCategory {{ Id = {animalCategory.Id}, Name = {animalCategory.Name ?? "null"} }}");
            }
            else if (record is ProductAnimalCategory productAnimalCategory)
            {
                Console.WriteLine($" - ProductAnimalCategory {{ ProductId = {productAnimalCategory.ProductId}, AnimalCategoryId = {productAnimalCategory.AnimalCategoryId} }}");
            }
            else
            {
                Console.WriteLine($" - {record}");
            }
        }

        return records;
    }
    public async Task ResetDatabaseAsync()
    {
        // Truncar tablas
        await _productsDbContext.Database.ExecuteSqlRawAsync(
            @"TRUNCATE TABLE products.""Products"", products.""AnimalCategories"", products.""ProductAnimalCategories"" CASCADE;"
        );
        await _usersDbContext.Database.ExecuteSqlRawAsync(
            @"TRUNCATE TABLE auth.""Roles"" CASCADE;"
        );

        // Reiniciar secuencias
        await _productsDbContext.Database.ExecuteSqlRawAsync(
            @"ALTER SEQUENCE products.""Products_Id_seq"" RESTART WITH 1;
              ALTER SEQUENCE products.""AnimalCategories_Id_seq"" RESTART WITH 1;
              ALTER SEQUENCE products.""ProductAnimalCategories_Id_seq"" RESTART WITH 1;"
        );
        await _usersDbContext.Database.ExecuteSqlRawAsync(
            @"ALTER SEQUENCE auth.""Roles_Id_seq"" RESTART WITH 1;"
        );

        Console.WriteLine("Database tables truncated and sequences reset successfully.");
    }
}

// Mapa para ProductAnimalCategory
public class ProductAnimalCategoryMap : ClassMap<ProductAnimalCategory>
{
    public ProductAnimalCategoryMap()
    {
        Map(m => m.ProductId).Name("ProductId");
        Map(m => m.AnimalCategoryId).Name("AnimalCategoryId");
        Map(m => m.Product).Ignore();
        Map(m => m.AnimalCategory).Ignore();
    }
}