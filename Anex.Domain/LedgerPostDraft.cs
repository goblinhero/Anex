using Anex.Domain.Abstract;
using Anex.Domain.Rules;

namespace Anex.Domain;

public class LedgerPostDraft : BaseEntity<LedgerPostDraft>
{
    public virtual DateOnly FiscalDate { get; set; }
    public virtual int VoucherNumber { get; set; }
    public virtual decimal Amount { get; set; }
    public virtual LedgerTag? LedgerTag { get; set; }
    public virtual LedgerTag? ContraTag { get; set; }
    public virtual LedgerDraft? LedgerDraft { get; private set; }
    public virtual bool IsReadyForBookkeeping(FiscalPeriod period, out IEnumerable<string> errors)
    {
        return new RuleSet<LedgerPostDraft>(GetBookkeepingRules()).UpholdsRules(this, out errors);
    }
    private IEnumerable<IRule<LedgerPostDraft>> GetBookkeepingRules()
    {
        yield return CannotBeNull(lpd => lpd.LedgerTag);
    }
}