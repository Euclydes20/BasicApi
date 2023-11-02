using Api.Domain.Users;
using Api.Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _dataContext;

        public UserRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<User> AddAsync(User user)
        {
            await _dataContext.User.AddAsync(user);

            await _dataContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _dataContext.User.Update(user);

            await _dataContext.SaveChangesAsync();

            return user;
        }

        public async Task DeleteAsync(User user)
        {
            _dataContext.User.Remove(user);

            await _dataContext.SaveChangesAsync();
        }

        public async Task<IList<User>> GetAsync()
        {
            return await _dataContext.User.AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetAsync(int userId)
        {
            return await _dataContext.User.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            return await _dataContext.User.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login!.ToLower() == login.ToLower());
        }

        public async Task<bool> ExistsAsync(Expression<Func<User, bool>> func)
        {
            return await _dataContext.User.AsNoTracking()
                .AnyAsync(func);
        }
    }
}