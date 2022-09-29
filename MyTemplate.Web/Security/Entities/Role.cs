using Microsoft.AspNetCore.Identity;

namespace MyTemplate.Web.Security.Entities;

public class Role : IdentityRole<Guid>
{
  public Role() : base()
  {
    UserRoles = Enumerable.Empty<UserRole>();
  }
  public Role(string roleName) : this()
  {
    Name = roleName;
  }

  public IEnumerable<UserRole> UserRoles { get; set; }
}
