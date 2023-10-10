using System.Security.Claims;

namespace Api.Domain.Secutiry
{
    public interface IClaimsRepository
    {
        /// <summary>
        /// Obtém o claims principal
        /// </summary>
        ClaimsPrincipal? CurrentClaimsPrincipal { get; }

        /// <summary>
        /// Obtém o id do usuário
        /// </summary>
        int? UserId { get; }
        
        /// <summary>
        /// Obtém o login do usuário
        /// </summary>
        string? UserLogin { get; }
        
        /// <summary>
        /// Obtém o nome do usuário
        /// </summary>
        string? UserName { get; }

        /// <summary>
        /// Obtém um indicador informando se o usuário é super
        /// </summary>
        bool? UserSuper { get; }

        /// <summary>
        /// Obtém um indicador informando se o usuário está com uma senha provisória
        /// </summary>
        bool? UserProvisoryPassword { get; }

        /// <summary>
        /// Obtém a data de expiração do token
        /// </summary>
        DateTime? TokenExpiration { get; }
        
        /// <summary>
        /// Obtém o id da conexão http atual
        /// </summary>
        string? CurrentHttpConnectionId { get; }
    }
}
