

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
    private readonly TokenHelper _tokenHelper;
    public AuthRepository(IConfiguration config, AuthContext context, HttpClient httpClient, TokenHelper tokenHelper)
    {
        _context = context;
        _config = config;
        _httpClient = httpClient;
        _tokenHelper = tokenHelper;
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
        var token = CreateToken(user.Id, user.Username);
        return token;
    }
    public async Task Register(CreateUserDto user)
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
        UserEntity userEnt = user.ToUserEntity();
        userEnt.PasswordHash = hashedPassword;
        await _context.Users.AddAsync(userEnt);
        await _context.SaveChangesAsync();
    }

    private string CreateToken(int id, string username)
    {
        List<Claim> claims = new List<Claim>(){
       new Claim(JwtRegisteredClaimNames.Sub,id.ToString()),
       new Claim (ClaimTypes.Name, username)
     };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
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

    public async Task<NamesEntity?> CreateName(NamesDto names)
    {
        NamesEntity namesEntity = names.ToNamesEntity();
        var userId = _tokenHelper.GetUserIdFromToken();
        if(userId != null)
        {
          Console.WriteLine(userId);
        }
        var user = _context.Users.Include(u => u.Names).SingleOrDefault(u => u.Id == userId);


        if (user == null)
        {
            return null;
        };

        if (names != null)
        {
            if (user.Names == null)
            {
                user.Names = new NamesEntity();
            }
            user.Names.firstname = names.firstname;
            user.Names.lastname = names.lastname;
        }
        await _context.Names.AddAsync(namesEntity);
        await _context.SaveChangesAsync();        
        user.NamesId = namesEntity.Id;
        await _context.SaveChangesAsync();
        return namesEntity;
    }

    public async Task<IEnumerable<UserEntity>> GetAllUsers()
    {
      var users = await _context.Users.Include(u => u.Names).ToListAsync();
      
      return users;
    }

}
