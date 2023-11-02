using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Domain.UserAuthorizations
{
    public sealed class UserAuthorization
    {
        public int Id { get; set; } = 0;
        public int UserId { get; set; } = 0;
        public string? AuthorizationCode { get; set; } = string.Empty;
        public int AuthorizationCondition { get; set; } = 0;

        [NotMapped]
        public string? AuthorizationTitle { get; set; } = string.Empty;
        [NotMapped]
        public string? AuthorizationDescription { get; set; } = string.Empty;
        [NotMapped]
        public int AuthorizationGroup { get; set; } = 0;

        internal void Validate()
        {
            if (UserId <= 0)
                throw new ArgumentNullException(nameof(UserId), "Usuário referente a autorização não informado/localizado.");

            if (string.IsNullOrEmpty(AuthorizationCode?.Trim()))
                throw new ArgumentNullException(nameof(AuthorizationCode), "Código da autorização não informado/localizado.");

            if (AuthorizationCode.Length > 50)
                throw new ArgumentNullException(nameof(AuthorizationCode), "Código da autorização muito grande, limite de caracteres é 50.");
        }
    }
}
