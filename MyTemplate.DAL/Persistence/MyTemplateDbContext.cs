using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace MyTemplate.DAL.Persistence;
public class MyTemplateDbContext : DbContext
{
    public MyTemplateDbContext(DbContextOptions<MyTemplateDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetCallingAssembly());
    }
}
