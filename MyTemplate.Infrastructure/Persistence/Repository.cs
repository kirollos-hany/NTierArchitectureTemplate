using Ardalis.Specification.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyTemplate.Domain.Common.Interfaces;
using MyTemplate.Domain.Interfaces;
using MyTemplate.Domain.Interfaces.Persistence;

namespace MyTemplate.Infrastructure.Persistence;

public class Repository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T> where T : class, IAggregateRoot
{
    public Repository(MyTemplateDbContext dbContext) : base(dbContext)
    {
    }
}