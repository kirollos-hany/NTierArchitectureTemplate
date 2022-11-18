using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyTemplate.Infrastructure.Persistence;

public class Repository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
    public Repository(DbContext dbContext) : base(dbContext)
    {
    }
}