using Api.Domain;
using Api.Domain.UserAuthorizations;
using Api.Domain.Users;
using Api.Security;
using System.Linq.Expressions;

namespace Api.Services.UserAuthorizations
{
    public class UserAuthorizationService : IUserAuthorizationService
    {
        private readonly IUserAuthorizationRepository _userAuthorizationRepository;
        private readonly ISharedService _sharedService;

        public UserAuthorizationService(IUserAuthorizationRepository userAuthorizationRepository, ISharedService sharedService)
        {
            _userAuthorizationRepository = userAuthorizationRepository;
            _sharedService = sharedService;
        }

        private async Task Validate(UserAuthorization userAuthorization)
        {
            if (userAuthorization is null)
                throw new ArgumentNullException(nameof(userAuthorization), "Dados da autorização é inválido.");

            userAuthorization.Validate();

            if (!await _sharedService.UserService!.ExistsAsync(u => u.Id == userAuthorization.UserId))
                throw new Exception("Usuário da autorização não localizado.");
        }

        private async Task Validate(List<UserAuthorization> userAuthorizations)
        {
            if (userAuthorizations is null || !userAuthorizations.Any())
                throw new ArgumentNullException(nameof(userAuthorizations), "Dados do autorização é inválido.");

            userAuthorizations.ForEach(ua => ua.Validate());
            
            var userIds = userAuthorizations.Select(ua => ua.UserId).Distinct();
            if (!await _sharedService.UserService!.ExistsAsync(u => userIds.Contains(u.Id)))
                throw new Exception("Usuário da autorização não localizado.");
        }


        public async Task<IList<UserAuthorization>> AddToNewUserAsync(int userId)
        {
            if (userId <= 0)
                throw new Exception("Usuário da autorização não localizado.");

            if (await ExistsAsync(ua => ua.UserId == userId))
                throw new Exception("Já existem autorizações vinculadas a este usuário.");

            var userAuthorizations = AuthorizationMap.AuthorizationList.Select(al => new UserAuthorization()
            {
                UserId = userId,
                AuthorizationCode = al.AuthorizationTypeCode,
                AuthorizationCondition = (int)AuthorizationCondition.NotAuthorized,
            }).ToList();

            return await AddAsync(userAuthorizations);
        }

        public async Task<UserAuthorization> AddAsync(UserAuthorization userAuthorization)
        {
            if (userAuthorization is null)
                throw new ArgumentNullException(nameof(userAuthorization), "Dados do autorização é inválido.");

            await Validate(userAuthorization);

            return await _userAuthorizationRepository.AddAsync(userAuthorization);
        }

        public async Task<IList<UserAuthorization>> AddAsync(List<UserAuthorization> userAuthorizations)
        {
            if (userAuthorizations is null || !userAuthorizations.Any())
                throw new ArgumentNullException(nameof(userAuthorizations), "Dados do autorização é inválido.");

            await Validate(userAuthorizations);

            return await _userAuthorizationRepository.AddAsync(userAuthorizations);
        }
        
        public async Task<UserAuthorization> UpdateAsync(UserAuthorization userAuthorization)
        {
            if (userAuthorization is null)
                throw new ArgumentNullException(nameof(userAuthorization), "Dados do autorização é inválido.");

            if (!await ExistsAsync(ua => ua.Id == userAuthorization.Id))
                throw new ArgumentNullException(nameof(userAuthorization), "Autorização do usuário não localizada.");

            await Validate(userAuthorization);

            return await _userAuthorizationRepository.UpdateAsync(userAuthorization);
        }

        public async Task<UserAuthorization?> GetAsync(int userAuthorizationId)
        {
            if (userAuthorizationId <= 0)
                throw new ArgumentException("O Id da autorização não foi informado.", nameof(userAuthorizationId));

            return await _userAuthorizationRepository.GetAsync(userAuthorizationId);
        }
        
        public async Task<IList<UserAuthorization>> GetByUserAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("O Id do usuário não foi informado.", nameof(userId));

            return await _userAuthorizationRepository.GetByUserAsync(userId);
        }

        public async Task<UserAuthorization?> GetByUserAndAuthorizationCodeAsync(int userId, string authorizationCode)
        {
            if (userId <= 0)
                throw new ArgumentException("O Id do usuário não foi informado.", nameof(userId));

            if (string.IsNullOrEmpty(authorizationCode?.Trim()))
                throw new ArgumentException("O código da autorização não foi informado.", nameof(authorizationCode));

            return await _userAuthorizationRepository.GetByUserAndAuthorizationCodeAsync(userId, authorizationCode);
        }
        public async Task<bool> UserIsAuthorizedAsync(int userId, string authorizationCode)
        {
            if (userId <= 0)
                throw new ArgumentException("O Id do usuário não foi informado.", nameof(userId));

            if (string.IsNullOrEmpty(authorizationCode?.Trim()))
                throw new ArgumentException("O código da autorização não foi informado.", nameof(authorizationCode));

            return await _userAuthorizationRepository.UserIsAuthorizedAsync(userId, authorizationCode);
        }

        public async Task<bool> ExistsAsync(Expression<Func<UserAuthorization, bool>> func)
        {
            return await _userAuthorizationRepository.ExistsAsync(func);
        }
    }
}