using Anex.Domain;
using NHibernate.Mapping.ByCode.Conformist;

namespace Anex.Api.Database.Mappings;
    
public class LedgerTagMapping : ClassMapping<LedgerTag>
{
    public LedgerTagMapping()
    {
        this.AddEntity<LedgerTagMapping, LedgerTag>();
        Property(e => e.Description, pm => pm.NotNullable(true));
        Property(e => e.Number);
    }
}
