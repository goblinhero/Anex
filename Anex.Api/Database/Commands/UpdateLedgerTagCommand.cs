using System.Linq.Expressions;
using System.Threading.Tasks;
using Anex.Api.Database.Commands.Abstract;
using Anex.Api.Database.Commands.Utilities;
using Anex.Api.Dto;
using Anex.Domain;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class UpdateLedgerTagCommand : BaseUpdateEntityCommand<LedgerTag>
{
    private readonly EditableLedgerTagDto _dto;

    public UpdateLedgerTagCommand(long id, int version, EditableLedgerTagDto dto)
        : base(id, version)
    {
        _dto = dto;
    }

    protected override Task<CommandResult> TryUpdateEntity(ISession session, LedgerTag entity)
    {
        entity.Description = _dto.Description;
        entity.Number = _dto.Number;
        return Task.FromResult(new CommandResult());
    }
}