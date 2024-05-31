using TodoApp.Models.Dtos;
using TodoApp.Services.Commons;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace TodoApp.Services.Concretes;

public class TokenService : ITokenService {

    private readonly IConfiguration _configuration;
    public TokenService(IConfiguration configuration) {
        _configuration = configuration;
    }

    public string GenerateAccessToken(TokenRequestDto dto) {
        var jwtAccessToken = new JwtSecurityToken(_configuration["JWT:Issuer"], _configuration["JWT:Audience"], claims: dto.Claims, signingCredentials: 
            new SigningCredentials(
                 new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"])), 
                 SecurityAlgorithms.HmacSha256Signature
            )
         );
        return new JwtSecurityTokenHandler().WriteToken(jwtAccessToken);
    }

    public string GenerateRefreshToken() {
        return Guid.NewGuid().ToString().ToLower();
    }
}
