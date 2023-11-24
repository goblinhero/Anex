using System;
using System.ComponentModel.DataAnnotations;
using Anex.Api.Dto.Abstract;

namespace Anex.Api.Dto;

public class LedgerPostDraftDto : EntityDto
{
    [Required]
    public long LedgerDraftId { get; set; }
    [Required]
    public DateOnly FiscalDate { get; set; }
    public int? VoucherNumber { get; set; }
    public decimal Amount { get; set; }
    public long? LedgerTagId { get; set; }
    public int? LedgerTagNumber { get; set; }
    public string? LedgerTagDescription { get;set; }
    public long? ContraTagId { get; set; }
    public int? ContraTagNumber { get; set; }
    public string? ContraTagDescription { get;set; }
}