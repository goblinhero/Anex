using Anex.Domain;
using NHibernate.Mapping.ByCode.Conformist;

namespace Anex.Api.Database.Mappings;

public class LedgerDraftMapping : ClassMapping<LedgerDraft>
{
    public LedgerDraftMapping()
    {
        this.AddEntity<LedgerDraftMapping, LedgerDraft>();
        Property(e => e.Description, e => e.NotNullable(true));
        ManyToOne(e => e.FiscalPeriod);
    }
}