﻿namespace Light.Domain.Entities;

/// <summary>
///     A base class for DDD Entities. Includes support for domain events dispatched post-persistence.
///     use string for type of ID and set default is NewGuid as string.
/// </summary>
public abstract class Entity : BaseEntity<string>
{
    protected Entity() => Id = LightId.NewId();
}

