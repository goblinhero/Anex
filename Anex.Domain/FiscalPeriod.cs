using Anex.Domain.Abstract;
using Anex.Domain.Rules;

namespace Anex.Domain;

public class FiscalPeriod : BaseEntity<FiscalPeriod>
{
    public virtual DateOnly StartDate { get; protected set; }
    public virtual DateOnly EndDate { get; protected set; }

    public static FiscalPeriod Create(DateOnly startDate, DateOnly endDate)
    {
        return new FiscalPeriod
        {
            StartDate = startDate,
            EndDate = endDate
        };
    }

    protected override IEnumerable<IRule<FiscalPeriod>> GetValidationRules()
    {
        yield return new RelayRule<FiscalPeriod>(fp => fp.EndDate <= fp.StartDate, $"{nameof(EndDate)} must be later than {nameof(StartDate)} for a {nameof(FiscalPeriod)}");
    }

    public override string ToString()
    {
        return $"{StartDate.ToShortDateString()} - {EndDate.ToShortDateString()}";
    }
}