using AuthProjWebApi.Models;
using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthProjWebApi.Auth
{


    public interface IJwtManager
    {
        Token GetToken(User user);
    }

    public class JwtManager : IJwtManager
    {

        private readonly IConfiguration iconfiguration;

        public JwtManager(IConfiguration iconfiguration)
        {
            this.iconfiguration = iconfiguration;
        }
        public Token GetToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(iconfiguration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim("Id", user.Id.ToString(),ClaimValueTypes.Integer),
                new Claim("Username", user.Username,ClaimValueTypes.String)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)

            };
            var tokenData = tokenHandler.CreateToken(tokenDescriptor);
            var token = new Token { AccessToken = tokenHandler.WriteToken(tokenData) };
            return token;
        }
    }


    public class Token
    {
        public string? AccessToken { get; set; }

    }
}
