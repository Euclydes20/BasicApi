using System.Linq.Expressions;

namespace Api.Domain.Users
{
    public interface IUserService
    {
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task RemoveAsync(int userId);
        Task RemoveAsync(User user);
        Task<IList<User>> GetAsync();
        Task<User?> GetAsync(int id);
        Task<User?> GetByLoginAsync(string login);
        Task UpdateLastLoginAsync(int userId);
        Task UpdateLastLoginAsync(User user);
        Task<bool> ExistingAsync(Expression<Func<User, bool>> func);
    }
}
