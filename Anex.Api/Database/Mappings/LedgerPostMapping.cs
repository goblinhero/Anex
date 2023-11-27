using Anex.Domain;
using NHibernate.Mapping.ByCode.Conformist;

namespace Anex.Api.Database.Mappings;

public class LedgerPostMapping : ClassMapping<LedgerPost>
{
    public LedgerPostMapping()
    {
        this.AddTransaction<LedgerPostMapping, LedgerPost>();
        Property(e => e.FiscalDate, e => e.NotNullable(true));
        Property(e => e.VoucherNumber);
        Property(e => e.Amount, e => e.NotNullable(true));
        ManyToOne(e => e.LedgerTag);
    }
}