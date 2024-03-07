using MyTemplate.Common.Dtos.Entity;
using Entity = MyTemplate.Domain.Entities.Entity;
namespace MyTemplate.Common.Mappings.Entities;

public static class EntityMappingExtensions
{
    public static EntityDto ToDto(this Entity entity)
    {
        return new EntityDto(entity.Id, entity.CreatedAt, entity.UpdatedAt);
    }

    public static Entity ToDomain(this EntityDto entityDto)
    {
        return new Entity
        {
            CreatedAt = entityDto.CreatedAt,
            Id = entityDto.Id,
            UpdatedAt = entityDto.UpdatedAt
        };
    }
}