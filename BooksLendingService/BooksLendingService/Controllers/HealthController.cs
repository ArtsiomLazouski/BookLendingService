using Microsoft.AspNetCore.Mvc;

namespace BooksLendingService.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get() => Ok("ok");
}
