using Ardalis.Specification;

namespace MyTemplate.Infrastructure.Persistence;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}