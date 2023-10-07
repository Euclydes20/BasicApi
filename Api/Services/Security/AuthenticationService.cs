using Api.Domain.Secutiry;
using Api.Domain.Users;
using Api.Models.Security;
using Api.Security;

namespace Api.Services.Security
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;

        public AuthenticationService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<TokenInfo> Authenticate(Authentication authentication)
        {
            if (authentication is null)
                throw new ArgumentNullException(nameof(authentication), "Solicitação de login inválida.");

            if (authentication is null)
                throw new ArgumentNullException(nameof(authentication.Login), "Login inválido.");

            if (authentication is null)
                throw new ArgumentNullException(nameof(authentication.Password), "Senha inválida.");

            var user = await _userService.GetByLoginAsync(authentication.Login)
                ?? throw new Exception("Credenciais inválidas.");

            if (authentication.Password != user.Password)
                throw new Exception("Credenciais inválidas.");

            var tokenInfo = TokenService.GenerateToken(user)
                ?? throw new Exception("Token inválido.");

            await _userService.UpdateLastLoginAsync(user);

            return tokenInfo;
        }

        public async Task Logout()
        {
            
        }
    }
}
