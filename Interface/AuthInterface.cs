
using jwtapp.Dto;
using jwtapp.Entities;

namespace jwtapp.Interface;
public interface IAuthInterface {
  Task<IEnumerable<UserEntity>> GetAllUsers();
  Task<string?> Login(LoginUserDto user);
  Task<UserEntity?> Register(CreateUserDto user);
  Task<NamesEntity?> CreateName(NamesDto names);  
}
