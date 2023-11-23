using System;
using System.ComponentModel.DataAnnotations;

namespace Anex.Api.Dto.Abstract;

public abstract class EntityDto : IEntityDto
{
    [Required]
    public long? Id { get; set; }
    [Required]
    public int Version { get; set; }
    [Required]
    public DateTime Created { get; set; }
}