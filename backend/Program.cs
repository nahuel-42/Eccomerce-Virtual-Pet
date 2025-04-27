using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using Backend.Shared.Services;
using Backend.Modules.Users.Application.Queries;
using Backend.Modules.Users.Application.Services;
using Backend.Modules.Users.Infrastructure.Persistence;

using Backend.Modules.Products.Application.Queries;
using Backend.Modules.Products.Application.Interfaces;
using Backend.Modules.Products.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la base de datos

// Misma conexion para todos los entornos
var conn = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<UsersDbContext>(opts =>
    opts.UseNpgsql(conn)); 

builder.Services.AddDbContext<ProductDbContext>(opts =>
    opts.UseNpgsql(conn));

// Habilitar controladores
builder.Services.AddControllers();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // URL del frontend
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var jwtKey = builder.Configuration["Jwt:Key"];
var key = Encoding.ASCII.GetBytes(jwtKey);

// Configuracion de interfaces
builder.Services.AddScoped<IProductQueries, ProductQueries>();

// Configuracion de servicios
builder.Services.AddScoped<ImporterService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<AuthService>();

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

// Configurar middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

// Usar CORS
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Mapear controladores
app.MapControllers();

// Ejecutar tareas de inicialización, como importar datos
using (var scope = app.Services.CreateScope())
{
    var importer = scope.ServiceProvider.GetRequiredService<ImporterService>();
    await importer.ImportAllAsync();
}

app.Run();
