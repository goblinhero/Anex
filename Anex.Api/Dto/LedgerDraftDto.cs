using System.ComponentModel.DataAnnotations;
using Anex.Api.Dto.Abstract;

namespace Anex.Api.Dto;

public class LedgerDraftDto : EntityDto
{
    [Required] 
    public string Description { get; set; } = string.Empty;
    public long? FiscalPeriodId { get; set; }
}