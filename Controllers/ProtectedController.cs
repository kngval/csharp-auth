
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace jwtapp.Controllers;

[Route("api/data")] 
[ApiController]
[Authorize]
public class ProtectedController : ControllerBase {


  [HttpGet("protected")]
  public string ProtectGreet([FromQuery] string token)
  {
    return token;
  }
}
