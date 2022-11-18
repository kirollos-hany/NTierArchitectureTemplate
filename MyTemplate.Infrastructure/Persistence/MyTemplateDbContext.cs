using System.Reflection;
using Microsoft.EntityFrameworkCore;
using MyTemplate.Domain.Entities;
#nullable disable
namespace MyTemplate.Infrastructure.Persistence;
public class MyTemplateDbContext : DbContext
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
