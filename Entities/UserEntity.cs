
namespace jwtapp.Entities;
public class UserEntity {
  public int Id { get;set; }
  public required string Username {get;set;}
  public required string PasswordHash {get;set;}
  public required string Role {get;set;} 
  public NamesEntity? Names {get;set;}
  
  //Foreign key for NamesEntity
  public int? NamesId {get;set;}
}
