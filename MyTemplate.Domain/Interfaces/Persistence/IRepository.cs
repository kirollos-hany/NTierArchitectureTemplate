using Ardalis.Specification;
using MyTemplate.Domain.Common.Interfaces;

namespace MyTemplate.Domain.Interfaces;

public interface IRepository<T> : IRepositoryBase<T> where T : class, IAggregateRoot
{
    
}