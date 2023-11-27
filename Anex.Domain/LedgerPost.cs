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
    public virtual int VoucherNumber { get; protected set; }
    public virtual DateOnly FiscalDate { get; protected set; }
    public virtual decimal Amount { get; protected set; }
    public virtual LedgerTag? LedgerTag { get; protected set; }
}