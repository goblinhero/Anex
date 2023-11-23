using Anex.Domain;
using NHibernate.Mapping.ByCode.Conformist;

namespace Anex.Api.Database.Mappings;

public class FiscalPeriodMapping : ClassMapping<FiscalPeriod>
{
    public FiscalPeriodMapping()
    {
        this.AddEntity<FiscalPeriodMapping, FiscalPeriod>();
        Property(e => e.StartDate, e => e.NotNullable(true));
        Property(e => e.EndDate, e => e.NotNullable(true));
    }
}