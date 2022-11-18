using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyTemplate.Domain.Common.Interfaces;

namespace MyTemplate.Domain.Interfaces.Persistence;

public class Repository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
    public Repository(DbContext dbContext) : base(dbContext)
    {
    }
}