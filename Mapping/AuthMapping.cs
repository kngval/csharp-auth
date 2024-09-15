
using jwtapp.Entities;
using jwtapp.Dto;
namespace jwtapp.Mapping;
public static class AuthMapping {

  public static UserEntity ToUserEntity(this CreateUserDto user)
  {
    return new UserEntity()
    {
      Username = user.Username,
      PasswordHash = user.PasswordHash
    };
  }


  public static NamesEntity ToNamesEntity(this NamesDto names)
  {
    return new NamesEntity()
    {
     firstname = names.firstname,
     lastname = names.lastname
    };
  }
}
