using System.Threading.Tasks;
using Anex.Api.Database.Commands.Abstract;
using Anex.Api.Database.Queries;
using Anex.Api.Dto;
using Anex.Domain;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class CreateLedgerDraftCommand : BaseCreateCommand<LedgerDraft>
{
    private readonly EditableLedgerDraftDto _dto;

    public CreateLedgerDraftCommand(EditableLedgerDraftDto dto)
    {
        _dto = dto;
    }

    protected override async Task<QueryResult<LedgerDraft>> CreateEntity(ISession session)
    {
        var ledgerDraft = LedgerDraft.Create(_dto.Description!);
        if (_dto.FiscalPeriodId.HasValue)
        {
            var fiscalPeriod = await session.GetAsync<FiscalPeriod>(_dto.FiscalPeriodId.Value);
            if (fiscalPeriod == null)
            {
                return new QueryResult<LedgerDraft>($"{nameof(FiscalPeriod)} not found with id: {_dto.FiscalPeriodId.Value}");
            }
            ledgerDraft.FiscalPeriod = fiscalPeriod;
        }
        return new QueryResult<LedgerDraft>(ledgerDraft);
    }
}