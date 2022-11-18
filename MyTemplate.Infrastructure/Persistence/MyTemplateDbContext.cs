using System.Reflection;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyTemplate.Domain.Common.Entities;
using MyTemplate.Domain.Entities;
using MyTemplate.Domain.Entities.Security;

#nullable disable
namespace MyTemplate.Infrastructure.Persistence;
public class MyTemplateDbContext : IdentityDbContext<User,
    Role,
    Guid,
    UserClaim,
    UserRole,
    UserLogin,
    RoleClaim,
    UserToken>
{
    public MyTemplateDbContext(DbContextOptions<MyTemplateDbContext> options) : base(options)
    {

    }

    public DbSet<Entity> Entities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
    }
}
