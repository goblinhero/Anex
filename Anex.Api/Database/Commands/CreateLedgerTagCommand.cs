using System.Threading.Tasks;
using Anex.Api.Dto;
using Anex.Domain;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class CreateLedgerTagCommand : IExecutableCommand
{
    private readonly EditableLedgerTagDto _dto;

    public CreateLedgerTagCommand(EditableLedgerTagDto dto)
    {
        _dto = dto;
    }

    public async Task<CommandResult> TryExecute(ISession session)
    {
        var ledgerTag = LedgerTag.Create(_dto.Description);
        ledgerTag.Number = _dto.Number;
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