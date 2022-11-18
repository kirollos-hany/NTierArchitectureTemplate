using Microsoft.AspNetCore.Identity;
using MyTemplate.Infrastructure.Persistence;

namespace MyTemplate.Web.Security.Entities;

public class User : IdentityUser<Guid>, IAggregateRoot
{
  public User()
  {
    Claims = Enumerable.Empty<UserClaim>();
    Logins = Enumerable.Empty<UserLogin>();
    Tokens = Enumerable.Empty<UserToken>();
    UserRoles = Enumerable.Empty<UserRole>();
  }
  public IEnumerable<UserClaim> Claims { get; set; }
  public IEnumerable<UserLogin> Logins { get; set; }
  public IEnumerable<UserToken> Tokens { get; set; }
  public IEnumerable<UserRole> UserRoles { get; set; }
}
