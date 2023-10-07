using Api.Domain.Users;
using Api.Infra.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Repositories.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext dataContext;

        public UserRepository(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<User> AddAsync(User user)
        {
            await dataContext.User.AddAsync(user);

            await dataContext.SaveChangesAsync();

            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            dataContext.User.Update(user);

            await dataContext.SaveChangesAsync();

            return user;
        }

        public async Task RemoveAsync(User user)
        {
            dataContext.User.Remove(user);

            await dataContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            return await dataContext.User.AsNoTracking()
                .ToListAsync();
        }

        public async Task<User?> GetAsync(int userId)
        {
            return await dataContext.User.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            return await dataContext.User.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Login.ToLower() == login.ToLower());
        }

        public async Task<bool> ExistingAsync(Expression<Func<User, bool>> func)
        {
            return await dataContext.User.AsNoTracking()
                    .AnyAsync(func);
        }
    }
}
