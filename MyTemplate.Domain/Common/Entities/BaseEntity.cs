using System;

namespace MyTemplate.Domain.Common.Entities;

public class BaseEntity
{
    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}

public class BaseEntity<TId> : BaseEntity where TId : notnull
{
    public TId Id { get; set; }
}