using Anex.Domain.Abstract;
using Anex.Domain.Rules;

namespace Anex.Domain;

public class EconomicTransaction : BaseTransaction<EconomicTransaction>
{
    private readonly IList<LedgerPost>? _ledgerPosts;
    protected EconomicTransaction() { }
    public EconomicTransaction(IEnumerable<LedgerPost> ledgerPosts)
    {
        _ledgerPosts = ledgerPosts.ToList();
    }
    public virtual IList<LedgerPost> LedgerPosts
    {
        get { return _ledgerPosts!; }
    }
    protected override IEnumerable<IRule<EconomicTransaction>> GetValidationRules()
    {
        yield return new RelayRule<EconomicTransaction>(ec => ec.LedgerPosts.Sum(lp => lp.Amount) != decimal.Zero, $"{GetType().Name} must balance.");
    }
}