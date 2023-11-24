using System;
using System.ComponentModel.DataAnnotations;

namespace Anex.Api.Dto;

public class EditableLedgerPostDraftDto
{
    [Required]
    public long LedgerDraftId { get; set; }
    [Required]
    public DateOnly FiscalDate { get; set; }
    public int? VoucherNumber { get; set; }
    [Required]
    public decimal Amount { get; set; }
    public long? LedgerTagId { get; set; }
    public long? ContraTagId { get; set; }
}