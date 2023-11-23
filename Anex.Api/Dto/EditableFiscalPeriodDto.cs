using System;
using System.ComponentModel.DataAnnotations;

namespace Anex.Api.Dto;

public class EditableFiscalPeriodDto
{
    [Required] 
    public DateOnly StartDate { get; set; }
    [Required]
    public DateOnly EndDate { get; set; }
}