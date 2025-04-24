using Microsoft.EntityFrameworkCore;

using Backend.Modules.Users.Domain.Entities;
using Backend.Modules.Users.Infrastructure.Persistence;

namespace Backend.Modules.Users.Application.Queries
{
    public static class UserQueries
    {
        // Busca un usuario por email, incluyendo su rol.
        public static Task<User> GetByEmailWithRoleAsync(UsersDbContext context, string email) =>
            context.Users
                   .Include(u => u.Role)
                   .FirstOrDefaultAsync(u => u.Email == email);

        // Comprueba si existe un usuario con este email.
        public static Task<bool> ExistsByEmailAsync(UsersDbContext context, string email) =>
            context.Users.AnyAsync(u => u.Email == email);
    }
}
