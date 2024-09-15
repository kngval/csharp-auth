using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using jwtapp.Entities;
using jwtapp.Dto;
using jwtapp.Repository;
using jwtapp.Interface;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;
namespace jwt.Controllers
{
    [Route("api/auth")]
    [ApiController]

    public class AuthController : ControllerBase
    {
      private readonly IAuthInterface _userRepo;

      public AuthController(IAuthInterface userRepo)
      {
        _userRepo = userRepo;
      }


      [HttpGet("greet")]
      public string Greet()
      {
        return "test successful";
      }

      [HttpGet]
      public async Task<ActionResult<IEnumerable<UserEntity>>> GetAllUsers()
      {
        var users = await _userRepo.GetAllUsers();

        return Ok(users);
      }

      [HttpPost("login")]
      public async Task<ActionResult<string>> Login([FromBody] CreateUserDto userDto)
      {
        var token = await _userRepo.Login(userDto); 
        if(token is null)
        {
          return BadRequest("Error login credentials");
        }
        return Ok(new {token});

      }

      [HttpPost("register")]
      public async Task<ActionResult<string>> Register(CreateUserDto userDto){
        await _userRepo.Register(userDto);


        return Ok("User Created Successfully");
      }
      [Authorize]
      [HttpGet("fake-data")]
      public async Task<ActionResult<Todo>> FetchFakeData()
      {
        using(HttpClient client = new HttpClient())
        {
          var url = "https://jsonplaceholder.typicode.com/todos/1";
          var response = await client.GetStringAsync(url);

          Todo? todo = JsonSerializer.Deserialize<Todo>(response);
          return Ok(todo);
        }
      }
      [Authorize] 
      [HttpPost("create-names")]
      public async Task<ActionResult<NamesEntity>> CreateNames(NamesDto names)
      {
        var userNames = await _userRepo.CreateName(names);
        if(userNames == null)
        {
          return BadRequest("Error Creating Names");
        }
        return Ok(userNames); 
      }

    }

    public class Todo {
      public int userId {get;set;}
      public int id {get;set;}
      public string title {get;set;} = string.Empty;
      public bool completed {get;set;}
    }
}
