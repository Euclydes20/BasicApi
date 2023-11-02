using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Domain.Users
{
    public sealed class UserAuthorization
    {
        public int UserId { get; set; } = 0;
        public string AuthorizationCode { get; set; } = string.Empty;
        public bool Authorized { get; set; } = false;

        [NotMapped]
        public string AuthorizationDescription { get; set; } = string.Empty;
        [NotMapped]
        public int AuthorizationGroup { get; set; } = 0;
    }
}
