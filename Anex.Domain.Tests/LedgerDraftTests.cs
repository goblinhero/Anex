using Xunit;

namespace Anex.Domain.Tests;

public class LedgerDraftTests
{
    [Fact]
    public void Should_Only_Process_Posts_With_Amount()
    {
        var ledgerDraft = LedgerDraft.Create("Description");
        ledgerDraft.FiscalPeriod = new FiscalPeriod();
        
        var postDraft = LedgerPostDraft.Create(ledgerDraft);
        postDraft.Amount = decimal.Zero;

        Assert.True(ledgerDraft.TryBookkeep(new[] { postDraft }, out var transactions, out _));
        Assert.True(transactions.Count == 0);
    }

    [Fact]
    public void Should_Fail_Without_FiscalPeriod()
    {
        var ledgerDraft = LedgerDraft.Create("Description");

        Assert.False(ledgerDraft.TryBookkeep(Array.Empty<LedgerPostDraft>(), out var transactions, out _));
    }
}