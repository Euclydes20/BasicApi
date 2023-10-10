using Api.Domain.Users;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Api.Security
{
    public class TokenService
    {
        public static readonly string SECRETKEY = "DC3D69B4DD132F8A3320A1BC61010F59C4AF7049";

        public static TokenInfo GenerateToken(User user)
        {
            if (user is null) 
                throw new ArgumentNullException(nameof(user), "Dados inválidos para gerar token.");

            if (string.IsNullOrEmpty(user.Password)) 
                throw new Exception("Senha inválida.");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SECRETKEY);
            var expires = DateTime.Now.AddHours(12);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, user.Login ?? string.Empty),
                    new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
                    new Claim(ClaimTypes.Role, user.Super.ToString()),
                    new Claim(nameof(User.ProvisoryPassword), user.ProvisoryPassword.ToString()),
                    new Claim(ClaimTypes.Expiration, expires.ToString())
                }),
                NotBefore = DateTime.Now,
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var textToken = tokenHandler.WriteToken(token);

            return new TokenInfo()
            {
                UserId = user.Id,
                UserName = user.Name ?? string.Empty,
                UserLogin = user.Login ?? string.Empty,
                UserSuper = user.Super,
                ProvisoryPassword = user.ProvisoryPassword,
                Token = textToken ?? string.Empty,
                Expires = expires,
            };
        }
    }
}
