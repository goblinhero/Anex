using Anex.Domain.Abstract;

namespace Anex.Domain;

public class LedgerDraft : BaseEntity<LedgerDraft>
{
    protected LedgerDraft()
    {
    }

    public static LedgerDraft Create(string description)
    {
        return new LedgerDraft
        {
            Description = description
        };
    }
    public virtual string Description { get; set; } = string.Empty;
    public virtual FiscalPeriod? FiscalPeriod { get; set; }
    public virtual bool TryBookkeep(IEnumerable<LedgerPostDraft> postDrafts, out ICollection<EconomicTransaction> transactions, out IEnumerable<string> errors)
    {
        var viablePostDrafts = postDrafts.Where(pd => pd.Amount != decimal.Zero).ToList();

        if (FiscalPeriod == null)
        {
            errors = new[] { $"{nameof(FiscalPeriod)} must be set before bookkeeping." };
            transactions = Array.Empty<EconomicTransaction>();
            return false;
        }

        if (!IsReadyForBookkeeping(viablePostDrafts, FiscalPeriod, out errors))
        {
            transactions = Array.Empty<EconomicTransaction>();
            return false;
        }

        var tempTransactions = new List<EconomicTransaction>();
        foreach (var postDraftGroup in viablePostDrafts.GroupBy(pd => pd.VoucherNumber))
        {
            var ledgerPosts = postDraftGroup
                .SelectMany(pd =>
                {
                    var post = new LedgerPost(pd.VoucherNumber, pd.FiscalDate, pd.Amount, pd.LedgerTag!);
                    if (pd.ContraTag == null) return new[] { post };
                    var contraPost = new LedgerPost(pd.VoucherNumber, pd.FiscalDate, -pd.Amount, pd.ContraTag);
                    return new[] { post, contraPost };
                }).ToList();
            tempTransactions.Add(new EconomicTransaction(ledgerPosts));
        }

        if (IsValid(tempTransactions, out errors))
        {
            transactions = tempTransactions;
            return true;
        }

        transactions = Array.Empty<EconomicTransaction>();
        return false;
    }

    private bool IsValid(IEnumerable<EconomicTransaction> transactions, out IEnumerable<string> errors)
    {
        var tempErrors = new List<string>();
        bool isValid = true;
        foreach (var transaction in transactions)
        {
            if (!transaction.IsValid(out var validationErrors))
            {
                tempErrors.AddRange(validationErrors);
                isValid = false;
            }
        }
        errors = tempErrors;
        return isValid;
    }

    private bool IsReadyForBookkeeping(IEnumerable<LedgerPostDraft> postDrafts, FiscalPeriod fiscalPeriod, out IEnumerable<string> errors)
    {
        var tempErrors = new List<string>();
        bool isReady = true;
        foreach (var postDraft in postDrafts)
        {
            if (!postDraft.IsReadyForBookkeeping(fiscalPeriod, out var bookkeepingErrors))
            {
                tempErrors.AddRange(bookkeepingErrors);
                isReady = false;
            }
        }
        errors = tempErrors;
        return isReady;
    }
}