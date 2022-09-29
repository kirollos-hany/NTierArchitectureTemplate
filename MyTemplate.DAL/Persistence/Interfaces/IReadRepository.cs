using Ardalis.Specification;

namespace MyTemplate.DAL.Persistence;

public interface IReadRepository<T> : IReadRepositoryBase<T> where T : class, IAggregateRoot
{
}