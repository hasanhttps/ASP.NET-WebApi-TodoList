using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using TodoApp.Models.Dtos;
using TodoApp.Services.Commons;

namespace TodoApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase {

    private readonly IAuthService _authService;
    private readonly RoleManager<IdentityRole> _roleManager;


    public AuthController(IAuthService authService, RoleManager<IdentityRole> roleManager) { 
        _authService = authService;
        _roleManager = roleManager;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody]LoginRequestDto dto) {
        if (!ModelState.IsValid) 
            return BadRequest(ModelState);

        var result = await _authService.Login(dto);

        if (result.StatusCode == HttpStatusCode.NotFound)
            return NotFound("User not found!");
        else if (result.StatusCode == HttpStatusCode.Unauthorized)
            return Unauthorized("Password is wrong!");

        return Ok(result);
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody]RegisterRequestDto dto) {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.Register(dto);

        if (!result)
            return BadRequest(ModelState);
        else
            return Ok(result);
    }

    [HttpPost("CreateRole")]
    public async Task<IActionResult> CreateRoles() { 
        await _roleManager.CreateAsync(new IdentityRole("Admin"));
        await _roleManager.CreateAsync(new IdentityRole("User"));
        await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
        return Ok("Roles Created");
    }
}
