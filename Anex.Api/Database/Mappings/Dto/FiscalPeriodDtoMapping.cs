using Anex.Api.Dto;
using NHibernate.Mapping.ByCode.Conformist;

namespace Anex.Api.Database.Mappings.Dto;

public class FiscalPeriodDtoMapping : ClassMapping<FiscalPeriodDto>
{
    public FiscalPeriodDtoMapping()
    {
        Table("fiscalperiod");
        this.AddEntityDto<FiscalPeriodDtoMapping, FiscalPeriodDto>();
        Property(m => m.StartDate);
        Property(m => m.EndDate);
    }
}