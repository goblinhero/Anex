using System.Threading.Tasks;
using Anex.Api.Database.Queries;
using Anex.Api.Dto;
using Anex.Domain;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class CreateLedgerTagCommand : BaseCreateCommand<LedgerTag>
{
    private readonly EditableLedgerTagDto _dto;

    public CreateLedgerTagCommand(EditableLedgerTagDto dto)
    {
        _dto = dto;
    }

    protected override Task<QueryResult<LedgerTag>> CreateEntity(ISession session)
    {
        var ledgerTag = LedgerTag.Create(_dto.Description);
        ledgerTag.Number = _dto.Number;
        return Task.FromResult(new QueryResult<LedgerTag>(ledgerTag));
    }
}