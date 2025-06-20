using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Backend.Shared.Services;
using Backend.Modules.Users.Application.Queries;
using Backend.Modules.Users.Application.Services;
using Backend.Modules.Users.Infrastructure.Persistence;
using Backend.Modules.Users.Application.Interfaces;
using Backend.Modules.Products.Infrastructure.Persistence;
using Backend.Modules.Products.Application.Interfaces;
using Backend.Modules.Products.Application.Queries;
using Backend.Modules.Orders.Infrastructure.Persistence;
using Backend.Modules.Orders.Application.Interfaces;
using Backend.Modules.Orders.Application.Queries;
using Backend.Modules.Orders.Application.Factories;
using Backend.Modules.Orders.Application.Events;
using Backend.Modules.Connection.Infrastructure.RabbitMQ;
using Backend.Modules.Connection.Infrastructure.Publishers;
using Backend.Modules.Connection.Domain.Services;
using Backend.Modules.Connection.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos
var baseConnectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// UsersDbContext
builder.Services.AddDbContext<UsersDbContext>(options =>
    options.UseNpgsql($"{baseConnectionString};Search Path=auth"));

// ProductsDbContext
builder.Services.AddDbContext<ProductsDbContext>(options =>
    options.UseNpgsql($"{baseConnectionString};Search Path=products"));

// OrdersDbContext
builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseNpgsql($"{baseConnectionString};Search Path=orders"));

// Habilitar controladores
builder.Services.AddControllers();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var jwtKey = builder.Configuration["Jwt:Key"];
var key = Encoding.ASCII.GetBytes(jwtKey);

// RabbitMQ Configuration
builder.Services.Configure<RabbitMQSettings>(builder.Configuration.GetSection("RabbitMQ"));
builder.Services.AddSingleton<RabbitMQSettings>(provider =>
{
    var settings = new RabbitMQSettings();
    builder.Configuration.GetSection("RabbitMQ").Bind(settings);
    return settings;
});
builder.Services.AddSingleton<IRabbitMQConnection>(provider =>
{
    var settings = provider.GetRequiredService<RabbitMQSettings>();
    return new RabbitMQConnection(settings);
});

// Connection Module - Message Services
builder.Services.AddScoped<IMessagePublisher, RabbitMQPublisher>();
builder.Services.AddScoped<IMessageConsumer, RabbitMQConsumer>();
builder.Services.AddScoped<IOrderEventPublisher, OrderEventPublisher>();

builder.Services.AddHostedService<MessageConsumerService>();

// Configuración de servicios
builder.Services.AddScoped<ImporterService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<OrderFactory>();
builder.Services.AddScoped<OrderUpdater>();

// Registrar IProductQueries y su implementación
builder.Services.AddScoped<IProductQueries, ProductQueries>();
builder.Services.AddScoped<IProductCommands, ProductCommands>();
builder.Services.AddScoped<IOrderQueries, OrderQueries>();
builder.Services.AddScoped<IOrderCommands, OrderCommands>();
builder.Services.AddScoped<IUserQueries, UserQueries>();
builder.Services.AddScoped<IRoleQueries, RoleQueries>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});

var app = builder.Build();

// 🔥 Validar conexiones antes de arrancar
using (var scope = app.Services.CreateScope())
{
    var usersDbContext = scope.ServiceProvider.GetRequiredService<UsersDbContext>();
    var productsDbContext = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
    var ordersDbContext = scope.ServiceProvider.GetRequiredService<OrdersDbContext>();

    if (await usersDbContext.Database.CanConnectAsync())
        Console.WriteLine("✅ Connected to Users database.");
    else
        Console.WriteLine("❌ Cannot connect to Users database.");

    if (await productsDbContext.Database.CanConnectAsync())
        Console.WriteLine("✅ Connected to Products database.");
    else
        Console.WriteLine("❌ Cannot connect to Products database.");

    if (await ordersDbContext.Database.CanConnectAsync())
        Console.WriteLine("✅ Connected to Orders database.");
    else
        Console.WriteLine("❌ Cannot connect to Orders database.");

    // Importador (lo dejás comentado si querés)
    var importer = scope.ServiceProvider.GetRequiredService<ImporterService>();
    await importer.ImportAllAsync();
}

// Configurar middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

app.Run();
