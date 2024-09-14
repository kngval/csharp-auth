
using jwtapp.Entities;
using jwtapp.Dto;
namespace jwtapp.Mapping;
public static class AuthMapping {

  public static UserEntity ToEntity(this CreateUserDto user)
  {
    return new UserEntity()
    {
      Username = user.Username,
      PasswordHash = user.PasswordHash
    };
  }
}
