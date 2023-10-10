using Api.Auxiliary;
using Api.Domain.Secutiry;
using Api.Domain.Users;
using System.Security.Claims;

namespace Api.Infra.Security
{
    public class ClaimsRepository : IClaimsRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public ClaimsPrincipal? CurrentClaimsPrincipal => _httpContextAccessor.HttpContext?.User;

        public int? UserId => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Sid)?.Value.To<int>();

        public string? UserLogin => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        public string? UserName => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Name)?.Value;

        public bool? UserSuper => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Role)?.Value.To<bool>();

        public bool? UserProvisoryPassword => _httpContextAccessor.HttpContext?.User.FindFirst(nameof(User.ProvisoryPassword))?.Value.To<bool>();

        public DateTime? TokenExpiration => _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.Expiration)?.Value.To<DateTime>();

        public string? CurrentHttpConnectionId => _httpContextAccessor.HttpContext?.Connection.Id;
    }
}
