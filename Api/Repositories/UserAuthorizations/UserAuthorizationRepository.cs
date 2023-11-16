using Api.Domain.UserAuthorizations;
using Api.Infra.Database;
using Api.Security;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Api.Repositories.UserAuthorizations
{
    public class UserAuthorizationRepository : IUserAuthorizationRepository
    {
        private readonly DataContextEF _dataContextEF;

        public UserAuthorizationRepository(DataContextEF dataContext)
        {
            this._dataContextEF = dataContext;
        }

        public async Task<UserAuthorization> AddAsync(UserAuthorization userAuthorization)
        {
            await _dataContextEF.UserAuthorization.AddAsync(userAuthorization);

            await _dataContextEF.SaveChangesAsync();

            return userAuthorization;
        }

        public async Task<IList<UserAuthorization>> AddAsync(List<UserAuthorization> userAuthorizations)
        {
            await _dataContextEF.UserAuthorization.AddRangeAsync(userAuthorizations);

            await _dataContextEF.SaveChangesAsync();

            return userAuthorizations;
        }

        public async Task<UserAuthorization> UpdateAsync(UserAuthorization userAuthorization)
        {
            _dataContextEF.UserAuthorization.Update(userAuthorization);

            await _dataContextEF.SaveChangesAsync();

            return userAuthorization;
        }

        public async Task<UserAuthorization?> GetAsync(int userAuthorizationId)
        {
            return await _dataContextEF.UserAuthorization.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == userAuthorizationId);
        }

        public async Task<IList<UserAuthorization>> GetByUserAsync(int userId)
        {
            return await _dataContextEF.UserAuthorization.AsNoTracking()
                .Where(ua => ua.UserId == userId)
                .ToListAsync();
        }

        public async Task<UserAuthorization?> GetByUserAndAuthorizationCodeAsync(int userId, string authorizationCode)
        {
            return await _dataContextEF.UserAuthorization.AsNoTracking()
                .FirstOrDefaultAsync(ua => 
                    ua.UserId == userId &&
                    ua.AuthorizationCode == authorizationCode);
        }

        public async Task<bool> UserIsAuthorizedAsync(int userId, string authorizationCode)
        {
            return await _dataContextEF.UserAuthorization.AsNoTracking()
                .AnyAsync(ua =>
                    ua.UserId == userId &&
                    ua.AuthorizationCode == authorizationCode &&
                    ua.AuthorizationCondition == (int)AuthorizationCondition.Authorized);
        }

        public async Task<bool> ExistsAsync(Expression<Func<UserAuthorization, bool>> func)
        {
            return await _dataContextEF.UserAuthorization.AsNoTracking()
                .AnyAsync(func);
        }
    }
}
