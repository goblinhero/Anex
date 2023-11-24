using Anex.Domain;
using NHibernate.Mapping.ByCode.Conformist;

namespace Anex.Api.Database.Mappings;

public class LedgerPostDraftMapping : ClassMapping<LedgerPostDraft>
{
    public LedgerPostDraftMapping()
    {
        this.AddEntity<LedgerPostDraftMapping, LedgerPostDraft>();
        Property(e => e.FiscalDate, e => e.NotNullable(true));
        Property(e => e.VoucherNumber);
        Property(e => e.Amount, e => e.NotNullable(true));
        ManyToOne(e => e.LedgerTag);
        ManyToOne(e => e.ContraTag);
        ManyToOne(e => e.LedgerDraft, e => e.NotNullable(true));
    }
}