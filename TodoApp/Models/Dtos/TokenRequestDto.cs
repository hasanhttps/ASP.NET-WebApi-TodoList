using System.Security.Claims;

namespace TodoApp.Models.Dtos;

public class TokenRequestDto {
    public string Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public IList<string> Roles { get; set; }
    public List<Claim> Claims { get; set; }
}
