using Anex.Domain.Abstract;
using Anex.Domain.Rules;

namespace Anex.Domain;

public class LedgerTag : Entity<LedgerTag>
{
    public virtual string Description { get; set; } = string.Empty;
    protected override IEnumerable<IRule<LedgerTag>> GetValidationRules()
    {
        yield return new RelayRule<LedgerTag>(lt => string.IsNullOrWhiteSpace(lt.Description), "Description cannot be empty for a ledger tag");
    }
}
