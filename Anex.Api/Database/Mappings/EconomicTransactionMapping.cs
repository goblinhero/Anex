using Anex.Domain;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Anex.Api.Database.Mappings;

public class EconomicTransactionMapping : ClassMapping<EconomicTransaction>
{
    public EconomicTransactionMapping()
    {
        this.AddTransaction<EconomicTransactionMapping, EconomicTransaction>();
        Bag(ec => ec.LedgerPosts, cm =>
        {
            cm.Cascade(Cascade.All.Include(Cascade.DeleteOrphans));
            cm.Lazy(CollectionLazy.Lazy);
            cm.Access(Accessor.Field);
            cm.Key(k => k.Column("economictransactionid"));
        }, cer => cer.OneToMany(s => s.Class(typeof(LedgerPost))));
    }
}