using System;
using System.ComponentModel.DataAnnotations;
using Anex.Api.Dto.Abstract;

namespace Anex.Api.Dto;

public class FiscalPeriodDto : EntityDto
{
    [Required]
    public DateOnly StartDate { get; set; }
    [Required]
    public DateOnly EndDate { get; set; }
}