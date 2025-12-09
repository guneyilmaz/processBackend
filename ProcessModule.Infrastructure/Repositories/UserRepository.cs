using Microsoft.EntityFrameworkCore;
using ProcessModule.Application.Interfaces;
using ProcessModule.Domain.Entities;
using ProcessModule.Persistence.Data;

namespace ProcessModule.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbSet.AnyAsync(u => u.Email == email && !u.IsDeleted);
    }

    public override async Task<User?> GetByIdAsync(int id)
    {
        return await _dbSet
            .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
    }

    public override async Task<IEnumerable<User>> GetAllAsync()
    {
        return await _dbSet
            .Where(u => !u.IsDeleted)
            .ToListAsync();
    }
}