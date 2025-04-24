using Microsoft.EntityFrameworkCore;

using Backend.Modules.Users.Domain.Entities;
using Backend.Modules.Users.Infrastructure.Persistence;

namespace Backend.Modules.Users.Application.Queries
{
    public static class RoleQueries
    {
        public static async Task<Role?> GetByIdAsync(UsersDbContext context, int id)
        {
            return await context.Roles.FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}