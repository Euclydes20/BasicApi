using System.Linq.Expressions;

namespace Api.Domain.UserAuthorizations
{
    public interface IUserAuthorizationRepository
    {
        Task<UserAuthorization> AddAsync(UserAuthorization userAuthorization);
        Task<IList<UserAuthorization>> AddAsync(List<UserAuthorization> userAuthorizations);
        Task<UserAuthorization> UpdateAsync(UserAuthorization userAuthorization);
        Task<UserAuthorization?> GetAsync(int userAuthorizationId);
        Task<IList<UserAuthorization>> GetByUserAsync(int userId);
        Task<UserAuthorization?> GetByUserAndAuthorizationCodeAsync(int userId, string authorizationCode);
        Task<bool> UserIsAuthorizedAsync(int userId, string authorizationCode);
        Task<bool> ExistsAsync(Expression<Func<UserAuthorization, bool>> func);
    }
}
