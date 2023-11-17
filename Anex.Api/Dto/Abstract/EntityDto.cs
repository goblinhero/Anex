using System;

namespace Anex.Api.Dto.Abstract;

public abstract class EntityDto : IEntityDto
{
    public long? Id { get; set; }
    public int Version { get; set; }
    public DateTime Created { get; set; }
}