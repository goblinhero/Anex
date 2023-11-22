using Anex.Domain.Abstract;
using Anex.Domain.Rules;

namespace Anex.Domain;

public class LedgerTag : BaseEntity<LedgerTag>
{
    protected LedgerTag() { }

    public static LedgerTag Create(string? description)
    {
        return new LedgerTag { Description = description };
    }

    public virtual string? Description { get; set; } = string.Empty;
    public virtual int? Number { get; set; }
    protected override IEnumerable<IRule<LedgerTag>> GetValidationRules()
    {
        yield return CannotBeEmpty(lt => lt.Description);
        yield return new RelayRule<LedgerTag>(lt => lt.Number.HasValue && lt.Number.Value <= 0, $"{nameof(Number)} cannot be negative for a {GetType().Name}");
    }
}