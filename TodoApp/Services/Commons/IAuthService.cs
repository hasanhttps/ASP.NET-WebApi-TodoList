using TodoApp.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Services.Commons;

public interface IAuthService {
    Task<LoginResponseDto> Login(LoginRequestDto dto);
    Task<bool> Register(RegisterRequestDto dto);
}
