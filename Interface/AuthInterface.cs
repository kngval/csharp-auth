
using jwtapp.Dto;
using jwtapp.Entities;

namespace jwtapp.Interface;
public interface IAuthInterface {
  Task<IEnumerable<UserEntity>> GetAllUsers();
  Task<string?> Login(CreateUserDto user);
  Task Register(CreateUserDto user);
  Task<NamesEntity?> CreateName(NamesDto names);  
}
