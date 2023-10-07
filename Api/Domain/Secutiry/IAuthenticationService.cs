using Api.Models.Security;
using Api.Security;

namespace Api.Domain.Secutiry
{
    public interface IAuthenticationService
    {
        Task<TokenInfo> Authenticate(Authentication authentication);
        Task Logout();
    }
}
