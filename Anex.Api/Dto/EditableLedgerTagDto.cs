using System.ComponentModel.DataAnnotations;

namespace Anex.Api.Dto;

public class EditableLedgerTagDto
{
    [Required] 
    public string Description { get; set; } = string.Empty;
    public int? Number { get; set; }
}