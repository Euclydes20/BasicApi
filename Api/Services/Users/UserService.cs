using Api.Domain.Users;
using Api.Security;
using System.Linq.Expressions;

namespace Api.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User> AddAsync(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user), "Dados do usuário é inválido.");

            user.CreationDate = DateTime.Now;
            user.Validate();

            if (await ExistingAsync(u => u.Login == user.Login))
                throw new Exception("Login já utilizado.");

            user.ProvisoryPassword = true;
            user.Password = user.Password.Hash();
            user.Blocked = false;

            return await _userRepository.AddAsync(user);
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

            if (await ExistingAsync(u => u.Id != user.Id && u.Login == user.Login))
                throw new Exception("Login já utilizado.");

            return await _userRepository.UpdateAsync(user);
        }

        public async Task RemoveAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("O Id do usuário não foi informado.", nameof(userId));

            await RemoveAsync(await GetAsync(userId));
        }

        public async Task RemoveAsync(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user), "Usuário não localizado.");

            if (user.Super)
                throw new Exception("Este usuário não pode ser removido.");

            await _userRepository.RemoveAsync(user);
        }

        public async Task<IEnumerable<User>> GetAsync()
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

        public async Task<bool> ExistingAsync(Expression<Func<User, bool>> func)
        {
            return await _userRepository.ExistingAsync(func);
        }
    }
}