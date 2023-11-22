using System.Linq.Expressions;
using System.Threading.Tasks;
using Anex.Api.Dto;
using Anex.Domain;
using Anex.Domain.Abstract;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class UpdateLedgerTagCommand : IExecutableCommand
{
    private readonly long _id;
    private readonly int _version;
    private readonly EditableLedgerTagDto _dto;

    public UpdateLedgerTagCommand(long id, int version, EditableLedgerTagDto dto)
    {
        _id = id;
        _version = version;
        _dto = dto;
    }

    public async Task<CommandResult> TryExecute(ISession session)
    {
        var ledgerTag = await session.GetAsync<LedgerTag>(_id);
        if (ledgerTag == null)
        {
            return CommandResult.NotFoundResult<LedgerTag>(_id);
        }

        if (ledgerTag.Version != _version)
        {
            return CommandResult.VersionConflict<LedgerTag>(_id, ledgerTag.Version, _version);
        }

        ledgerTag.Description = _dto.Description;
        ledgerTag.Number = _dto.Number;
        return !ledgerTag.IsValid(out var errors) 
            ? new CommandResult(errors) 
            : new CommandResult();
    }
}