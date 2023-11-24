using System.ComponentModel.DataAnnotations;

namespace Anex.Api.Dto;

public class EditableLedgerDraftDto
{
    [Required] 
    public string Description { get; set; } = string.Empty;
    public long? FiscalPeriodId { get; set; }
}