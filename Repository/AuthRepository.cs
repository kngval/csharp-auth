

using jwtapp.Data;
using jwtapp.Dto;
using jwtapp.Entities;
using jwtapp.Mapping;
using jwtapp.Interface;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace jwtapp.Repository;

public class AuthRepository : IAuthInterface
{
    private readonly AuthContext _context;
    private readonly IConfiguration _config;
    private readonly HttpClient _httpClient;
    public AuthRepository(IConfiguration config,AuthContext context,HttpClient httpClient)
    {
        _context = context;
        _config = config;
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<UserEntity>> GetAllUsers()
    {
        var users = await _context.Users.ToListAsync();
        return users;
    }

    public async Task<string?> Login(CreateUserDto userDto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDto.Username);
        if (user is null)
        {
            return null;
        }
        var passwordValid = BCrypt.Net.BCrypt.Verify(userDto.PasswordHash, user.PasswordHash);
        if (!passwordValid)
        {
            return null;
        }
        var token = CreateToken(user.Id,user.Username); 
        return token;
    }
    public async Task Register(CreateUserDto user)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        UserEntity userEnt = user.ToEntity();
        userEnt.PasswordHash = hashedPassword;
        await _context.Users.AddAsync(userEnt);
        await _context.SaveChangesAsync();
    }

    private string CreateToken(int id,string username)
    {
     List<Claim> claims = new List<Claim>(){
       new Claim(JwtRegisteredClaimNames.Sub,id.ToString()),
       new Claim (ClaimTypes.Name, username)
     }; 

     var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value!));
     var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

     var tokenDescriptor = new SecurityTokenDescriptor{
       Subject = new ClaimsIdentity(claims),
       Expires = DateTime.Now.AddHours(1),
       SigningCredentials = creds,
       Issuer = _config.GetSection("Jwt:Issuer").Value,
       Audience = _config.GetSection("Jwt:Audience").Value
     };

     var tokenHandler = new JwtSecurityTokenHandler();
     var token = tokenHandler.CreateToken(tokenDescriptor);

     return tokenHandler.WriteToken(token);
    }
    
    public async Task<UserEntity> CreateName(NamesDto names)
    {
      
    }

}
