using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Backend.Modules.Users.Application.DTOs;
using Backend.Modules.Users.Domain.Entities;
using Backend.Modules.Users.Infrastructure.Persistence;
using Backend.Modules.Users.Application.Queries;
using Backend.Shared.Services;

namespace Backend.Modules.Users.Application.Services {

    public class AuthService
    {
        private readonly UsersDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly PasswordService _passwordService;

        public AuthService(UsersDbContext context,
                           IConfiguration configuration,
                           PasswordService passwordService)
        {
            _context = context;
            _configuration = configuration;
            _passwordService = passwordService;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var user = await UserQueries.GetByEmailWithRoleAsync(_context, request.Email);
            if (user == null)
            {
                Console.WriteLine($"Login failed: User with email {request.Email} not found.");
                return null;
            }

            // Console.WriteLine($"Verifying password for user {user.Email}, PasswordHash: {user.PasswordHash ?? "NULL"}");
            if (!_passwordService.VerifyPassword(user, user.PasswordHash, request.Password))
            {
                Console.WriteLine($"Login failed: Invalid password for user {user.Email}.");
                return null;
            }

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

            return new LoginResponse
            {
                Token = tokenHandler.WriteToken(token),
                ExpiresIn = 3600,
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Role = new RoleDto
                    {
                        Id = user.Role.Id,
                        Name = user.Role.Name
                    }
                }
            };
        }

        public async Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        {
            if (await UserQueries.ExistsByEmailAsync(_context, request.Email))
                return new RegisterResponse
                {
                    Message = "User already exists"
                };

            var user = new User(request.Name,request.Email);
            var hashed = _passwordService.HashPassword(user, request.Password);
            user.SetPassword(hashed);

            // Obtener el rol por defecto (ID = 2) desde la base de datos usando RoleQueries
            var defaultRole = await RoleQueries.GetByIdAsync(_context, 2);
            if (defaultRole == null)
                return new RegisterResponse
                {
                    Message = "Default role not found"
                };

            user.SetRole(defaultRole);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new RegisterResponse
            {
                Message = "User registered successfully"
            };
        }
    }
}
