using Anex.Api.Dto;
using NHibernate.Mapping.ByCode.Conformist;

namespace Anex.Api.Database.Mappings.Dto;

public class LedgerPostDraftDtoMapping : ClassMapping<LedgerPostDraftDto>
{
    public LedgerPostDraftDtoMapping()
    {
        this.AddEntityDto<LedgerPostDraftDtoMapping, LedgerPostDraftDto>();
        Property(m => m.FiscalDate);
        Property(m => m.VoucherNumber);
        Property(m => m.Amount);
        Property(m => m.LedgerDraftId);
        Property(m => m.LedgerTagId);
        Property(m => m.LedgerTagNumber);
        Property(m => m.LedgerTagDescription);
        Property(m => m.ContraTagId);
        Property(m => m.ContraTagNumber);
        Property(m => m.ContraTagDescription);
    }
}