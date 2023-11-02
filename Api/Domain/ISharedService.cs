using Api.Domain.UserAuthorizations;
using Api.Domain.Users;

namespace Api.Domain
{
    public interface ISharedService
    {
        IUserService? UserService { get; }
        IUserAuthorizationService? UserAuthorizationService { get; }
    }
}
