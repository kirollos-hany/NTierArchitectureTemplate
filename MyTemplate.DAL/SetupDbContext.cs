using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyTemplate.DAL.Persistence;

namespace MyTemplate.DAL;

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