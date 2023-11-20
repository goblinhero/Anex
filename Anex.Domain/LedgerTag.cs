using Anex.Domain.Abstract;
using Anex.Domain.Rules;

namespace Anex.Domain;

public class LedgerTag : BaseEntity<LedgerTag>
{
    protected LedgerTag() { }

    public static LedgerTag Create(string description)
    {
        return new LedgerTag { Description = description };
    }

    public virtual string Description { get; set; } = string.Empty;
    protected override IEnumerable<IRule<LedgerTag>> GetValidationRules()
    {
        yield return CannotBeEmpty(lt => lt.Description);
    }
}