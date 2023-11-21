using System.Threading.Tasks;
using Anex.Api.Dto;
using Anex.Domain;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class CreateLedgerTagCommand : IExecutableCommand
{
    private readonly LedgerTagDto _dto;

    public CreateLedgerTagCommand(LedgerTagDto dto)
    {
        _dto = dto;
    }

    public async Task<CommandResult> TryExecute(ISession session)
    {
        var ledgerTag = LedgerTag.Create(_dto.Description);
        if (!ledgerTag.IsValid(out var errors))
        {
            return new CommandResult(errors);
        }

        await session.SaveAsync(ledgerTag);
        AssignedId = ledgerTag.Id;
        return new CommandResult();
    }
    public long? AssignedId { get; private set; }
}