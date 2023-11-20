using NSubstitute;
using Xunit;

namespace Anex.Domain.Tests;

public class LedgerTagTests
{
    [Fact]
    public void Invalid_When_Description_IsEmpty()
    {
        var ledgerTag = LedgerTag.Create(string.Empty);
        Assert.False(ledgerTag.IsValid(out _));
    }

    [Fact]
    public void Valid_When_Description_IsFilled()
    {
        var ledgerTag = LedgerTag.Create("Literally anything");
        Assert.True(ledgerTag.IsValid(out _));
    }
}
