using TodoApp.Contexts;
using TodoApp.Models.Dtos;
using TodoApp.Services.Commons;
using Microsoft.AspNetCore.Identity;
using TodoApp.Models.Entities.Concretes;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Services.Concretes;

public class AuthService : IAuthService {

    private readonly ITokenService _tokenService;
    private readonly AppDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signinManager;

    public AuthService(ITokenService tokenService, AppDbContext context, UserManager<User> userManager, SignInManager<User> signInManager) {
        _tokenService = tokenService;
        _context = context;
        _userManager = userManager;
        _signinManager = signInManager;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto dto) {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user is null)
            return new LoginResponseDto() { StatusCode = HttpStatusCode.NotFound };

        var result = await _signinManager.CheckPasswordSignInAsync(user , dto.Password, false);
        if (!result.Succeeded)
            return new LoginResponseDto() { StatusCode = HttpStatusCode.Unauthorized };

        var tokenRequestDto = new TokenRequestDto() {
            Email = dto.Email,
            Id = user.Id,
            Username = user.UserName,
            Roles = await _userManager.GetRolesAsync(user),
            Claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, string.Join(",", await _userManager.GetRolesAsync(user)))
            }
        };

        var accessToken = _tokenService.GenerateAccessToken(tokenRequestDto);

        return new LoginResponseDto() {
            AccessToken = accessToken,
            RefreshToken = null,
            StatusCode = HttpStatusCode.OK
        };
    }

    public async Task<bool> Register(RegisterRequestDto dto) {
        var existUser = await _userManager.FindByEmailAsync(dto.Email);
        if (existUser is not null)
            return false;

        var user = new User() {
            Email = dto.Email,
            UserName = dto.Username,
            Name = dto.FirstName,
            Surname = dto.LastName
        };
        var refreshToken = _tokenService.GenerateRefreshToken();
        var result = await _userManager.CreateAsync(user, dto.Password);
        return result.Succeeded;
    }
}
