using Api.Domain.UserAuthorizations;
using Api.Domain.Users;
using Api.Security;
using System.Linq.Expressions;
using System.Transactions;

namespace Api.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAuthorizationService _userAuthorizationService;

        public UserService(IUserRepository userRepository, IUserAuthorizationService userAuthorizationService)
        {
            _userRepository = userRepository;
            _userAuthorizationService = userAuthorizationService;
        }

        public async Task<User> AddAsync(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user), "Dados do usuário é inválido.");

            user.Id = 0;
            user.CreationDate = DateTime.Now;
            user.Validate();

            if (await ExistsAsync(u => u.Login == user.Login))
                throw new Exception("Login já utilizado.");

            user.ProvisoryPassword = true;
            user.Password = user.Password?.Hash() ?? string.Empty;
            user.Blocked = false;

            using (var transation = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                user = await _userRepository.AddAsync(user);
                await _userAuthorizationService.AddToNewUserAsync(user.Id);

                transation.Complete();
            }

            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user), "Dados do usuário é inválido.");

            var userSaved = await GetAsync(user.Id)
                ?? throw new ArgumentNullException(nameof(user), "Usuário não localizado.");

            if (userSaved.Super)
                throw new Exception("Este usuário não pode ser alterado.");

            user.CreationDate = userSaved.CreationDate;
            user.LastLogin = userSaved.LastLogin;
            user.Password = userSaved.Password;
            user.Validate();

            if (await ExistsAsync(u => u.Id != user.Id && u.Login == user.Login))
                throw new Exception("Login já utilizado.");

            return await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("O Id do usuário não foi informado.", nameof(userId));

            await DeleteAsync(await GetAsync(userId));
        }

        public async Task DeleteAsync(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user), "Usuário não localizado.");

            if (user.Super)
                throw new Exception("Este usuário não pode ser removido.");

            await _userRepository.DeleteAsync(user);
        }

        public async Task<IList<User>> GetAsync()
        {
            return await _userRepository.GetAsync();
        }

        public async Task<User?> GetAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("O Id do usuário não foi informado.", nameof(userId));

            return await _userRepository.GetAsync(userId);
        }

        public async Task<User?> GetByLoginAsync(string login)
        {
            if (string.IsNullOrEmpty(login))
                return null;

            return await _userRepository.GetByLoginAsync(login);
        }

        public async Task UpdateLastLoginAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("O Id do usuário não foi informado.", nameof(userId));

            await UpdateLastLoginAsync(await GetAsync(userId));
        }

        public async Task UpdateLastLoginAsync(User user)
        {
            if (user is null)
                throw new ArgumentException("Dados do usuário é inválido.", nameof(user));

            var userSaved = await GetAsync(user.Id);
            if (userSaved is null)
                throw new ArgumentNullException(nameof(userSaved), "Usuário não localizado.");

            userSaved.LastLogin = DateTime.Now;

            await _userRepository.UpdateAsync(userSaved);
        }

        public async Task<bool> ExistsAsync(Expression<Func<User, bool>> func)
        {
            return await _userRepository.ExistsAsync(func);
        }
    }
}