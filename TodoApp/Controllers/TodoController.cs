using Microsoft.AspNetCore.Mvc;

namespace TodoApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase {

    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAll() {
        return Ok("Her sey Okaydir");
    }
}
