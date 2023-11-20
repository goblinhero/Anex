using Anex.Domain.Abstract;

namespace Anex.Domain;

public class LedgerPost : BaseTransaction<LedgerPost>
{
    protected LedgerPost() { }
    internal LedgerPost(int voucherNumber, DateOnly fiscalDate, decimal amount, LedgerTag ledgerTag)
    {
        FiscalDate = fiscalDate;
        Amount = amount;
        LedgerTag = ledgerTag;
        VoucherNumber = voucherNumber;
    }
    public virtual int VoucherNumber { get; private set; }
    public virtual DateOnly FiscalDate { get; private set; }
    public virtual decimal Amount { get; private set; }
    public virtual LedgerTag? LedgerTag { get; private set; }
}