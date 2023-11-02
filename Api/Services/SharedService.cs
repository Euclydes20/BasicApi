using Api.Domain;
using Api.Domain.UserAuthorizations;
using Api.Domain.Users;

namespace Api.Services
{
    public class SharedService : ISharedService
    {
        private readonly IServiceProvider _serviceProvider;

        public SharedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private IUserService? _userService;
        public IUserService UserService
        {
            get
            {
                _userService ??= _serviceProvider.GetRequiredService<IUserService>();
                return _userService;
            }
            private set { }
        }
        
        private IUserAuthorizationService? _userAuthorizationService;
        public IUserAuthorizationService UserAuthorizationService
        {
            get
            {
                _userAuthorizationService ??= _serviceProvider.GetRequiredService<IUserAuthorizationService>();
                return _userAuthorizationService;
            }
            private set { }
        }
    }
}
