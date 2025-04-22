using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Data;
using Backend.Models;

// Controlador de autenticación que maneja el login y registro de usuarios.
[Route("user")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly OrdersDbContext _context;
    private readonly IConfiguration _configuration;
     private readonly PasswordService _passwordService;

    // Constructor del controlador de autenticación.
    // param: context - Contexto de base de datos
    // param: configuration - Configuración del sistema
    public AuthController(OrdersDbContext context, IConfiguration configuration, PasswordService passwordService)
    {
        _context = context;
        _configuration = configuration;
        _passwordService = passwordService;
    }

    // Endpoint para el inicio de sesión de usuario.
    // param: request - Datos de inicio de sesión (usuario y contraseña)
    // returns: Token JWT y datos del usuario si las credenciales son válidas
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Buscar usuario por email
        var user = _context.Users.FirstOrDefault(u => u.Email == request.Username);
        
        if (user == null)
            return Unauthorized(new { message = "Invalid credentials" });

        // Verificar si la contraseña coincide con el hash
        var isPasswordValid = _passwordService.VerifyPassword(user, user.PasswordHash, request.Password);

        if (!isPasswordValid)
            return Unauthorized(new { message = "Invalid credentials" });

        // Crear token JWT
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("id", user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role.Name)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return Ok(new
        {
            token = tokenHandler.WriteToken(token),
            expiresIn = 3600,
            user = new
            {
                id = user.Id,
                name = user.Name,
                role = new { id = user.Role.Id, name = user.Role.Name }
            }
        });
    }

    // Endpoint para registrar un nuevo usuario en el sistema.
    // param: request - Datos del usuario a registrar
    // returns: Mensaje indicando éxito o error
    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest request)
    {
        // Verificar si el usuario ya existe
        var exists = _context.Users.Any(u => u.Email == request.Email);
        if (exists)
            return BadRequest(new { message = "User already exists" });

        // Crear nuevo usuario
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = _passwordService.HashPassword(null, request.Password),
            RoleId = 2 // cliente por defecto
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        return Ok(new { message = "User registered" });
    }
}
