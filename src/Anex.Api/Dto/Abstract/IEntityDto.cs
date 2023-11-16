using System;

namespace Anex.Api.Dto.Abstract;

public interface IEntityDto
{
    long? Id { get; set; }
    int Version { get; set; }
    DateTime Created { get; set; }
}