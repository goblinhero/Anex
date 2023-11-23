using Anex.Api.Dto;
using NHibernate.Mapping.ByCode.Conformist;

namespace Anex.Api.Database.Mappings.Dto;

public class LedgerTagDtoMapping: ClassMapping<LedgerTagDto>
{
    public LedgerTagDtoMapping()
    {
        Table("ledgertag");
        this.AddEntityDto<LedgerTagDtoMapping, LedgerTagDto>();
        Property(m => m.Description);
        Property(m => m.Number);
    }
}
