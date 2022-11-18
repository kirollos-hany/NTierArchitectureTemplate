using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyTemplate.Infrastructure.Persistence;

namespace MyTemplate.Infrastructure;

public static class SetupDbContext
{
    public static void AddDbContext(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddDbContextPool<MyTemplateDbContext>(options => 
        {
            options.UseSqlServer(connectionString, opts => 
            {
               opts.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery); 
            });
        });
    }
}