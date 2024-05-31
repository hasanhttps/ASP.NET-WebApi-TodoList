using TodoApp.Models.Dtos;

namespace TodoApp.Services.Commons;

public interface ITokenService {
    public string GenerateAccessToken(TokenRequestDto dto);
    public string GenerateRefreshToken();
}
