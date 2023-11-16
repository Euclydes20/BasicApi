using Api.Domain.Users;
using Api.Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContextEF _dataContextEF;

        public UserRepository(DataContextEF dataContext)
        {
            this._dataContextEF = dataContext;
        }

        public async Task<User> AddAsync(User user)
        {
            await _dataContextEF.User.AddAsync(user);

            await _dataContextEF.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _dataContextEF.User.Update(user);

            await _dataContextEF.SaveChangesAsync();

            return user;
        }

        public async Task DeleteAsync(User user)
        {
            _dataContextEF.User.Remove(user);

            await _dataContextEF.SaveChangesAsync();
        }

        public async Task<IList<User>> GetAsync()
        {
            return await _dataContextEF.User.AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetAsync(int userId)
        {
            return await _dataContextEF.User.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            return await _dataContextEF.User.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login!.ToLower() == login.ToLower());
        }

        public async Task<bool> ExistsAsync(Expression<Func<User, bool>> func)
        {
            return await _dataContextEF.User.AsNoTracking()
                .AnyAsync(func);
        }
    }
}