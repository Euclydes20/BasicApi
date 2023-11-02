using Api.Domain.UserAuthorizations;
using Api.Infra.Database;
using Api.Security;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Repositories.UserAuthorizations
{
    public class UserAuthorizationRepository : IUserAuthorizationRepository
    {
        private readonly DataContext _dataContext;

        public UserAuthorizationRepository(DataContext dataContext)
        {
            this._dataContext = dataContext;
        }

        public async Task<UserAuthorization> AddAsync(UserAuthorization userAuthorization)
        {
            await _dataContext.UserAuthorization.AddAsync(userAuthorization);

            await _dataContext.SaveChangesAsync();

            return userAuthorization;
        }

        public async Task<IList<UserAuthorization>> AddAsync(List<UserAuthorization> userAuthorizations)
        {
            await _dataContext.UserAuthorization.AddRangeAsync(userAuthorizations);

            await _dataContext.SaveChangesAsync();

            return userAuthorizations;
        }

        public async Task<UserAuthorization> UpdateAsync(UserAuthorization userAuthorization)
        {
            _dataContext.UserAuthorization.Update(userAuthorization);

            await _dataContext.SaveChangesAsync();

            return userAuthorization;
        }

        public async Task<UserAuthorization?> GetAsync(int userAuthorizationId)
        {
            return await _dataContext.UserAuthorization.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userAuthorizationId);
        }

        public async Task<IList<UserAuthorization>> GetByUserAsync(int userId)
        {
            return await _dataContext.UserAuthorization.AsNoTracking()
                .Where(ua => ua.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserAuthorization?> GetByUserAndAuthorizationCodeAsync(int userId, string authorizationCode)
        {
            return await _dataContext.UserAuthorization.AsNoTracking()
                .FirstOrDefaultAsync(ua => 
                    ua.UserId == userId &&
                    ua.AuthorizationCode == authorizationCode);
        }

        public async Task<bool> UserIsAuthorizedAsync(int userId, string authorizationCode)
        {
            return await _dataContext.UserAuthorization.AsNoTracking()
                .AnyAsync(ua =>
                    ua.UserId == userId &&
                    ua.AuthorizationCode == authorizationCode &&
                    ua.AuthorizationCondition == (int)AuthorizationCondition.Authorized);
        }

        public async Task<bool> ExistsAsync(Expression<Func<UserAuthorization, bool>> func)
        {
            return await _dataContext.UserAuthorization.AsNoTracking()
                .AnyAsync(func);
        }
    }
}
