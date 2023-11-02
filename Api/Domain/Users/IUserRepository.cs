using System.Linq.Expressions;

namespace Api.Domain.Users
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);
        Task<User> UpdateAsync(User user);
        Task DeleteAsync(User user);
        Task<IList<User>> GetAsync();
        Task<User?> GetAsync(int userId);
        Task<User?> GetByLoginAsync(string login);
        Task<bool> ExistsAsync(Expression<Func<User, bool>> func);
    }
}
