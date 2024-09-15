
namespace jwtapp.Entities;
public class UserEntity {
  public int Id { get;set; }
  public string Username {get;set;} = string.Empty;
  public string PasswordHash {get;set;} = string.Empty;
  public NamesEntity? Names {get;set;}
  
  //Foreign key for NamesEntity
  public int? NamesId {get;set;}
}
