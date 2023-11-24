using Anex.Api.Dto;
using NHibernate.Mapping.ByCode.Conformist;

namespace Anex.Api.Database.Mappings.Dto;

public class LedgerDraftDtoMapping : ClassMapping<LedgerDraftDto>
{
    public LedgerDraftDtoMapping()
    {
        Table("LedgerDraft");
        this.AddEntityDto<LedgerDraftDtoMapping, LedgerDraftDto>();
        Property(m => m.Description);
        Property(m => m.FiscalPeriodId);
    }
}