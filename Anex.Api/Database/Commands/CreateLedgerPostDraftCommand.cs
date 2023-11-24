using System.Threading.Tasks;
using Anex.Api.Database.Commands.Abstract;
using Anex.Api.Database.Queries;
using Anex.Api.Dto;
using Anex.Domain;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class CreateLedgerPostDraftCommand : BaseCreateCommand<LedgerPostDraft>
{
    private readonly EditableLedgerPostDraftDto _dto;

    public CreateLedgerPostDraftCommand(EditableLedgerPostDraftDto dto)
    {
        _dto = dto;
    }

    protected override async Task<QueryResult<LedgerPostDraft>> CreateEntity(ISession session)
    {
        var ledgerDraft = await session.GetAsync<LedgerDraft>(_dto.LedgerDraftId);
        if (ledgerDraft == null)
        {
            return new QueryResult<LedgerPostDraft>($"No {nameof(LedgerDraft)} found with id: {_dto.LedgerDraftId}");
        }

        var ledgerPostDraft = LedgerPostDraft.Create(ledgerDraft);
        ledgerPostDraft.Amount = _dto.Amount;
        ledgerPostDraft.VoucherNumber = _dto.VoucherNumber;
        ledgerPostDraft.FiscalDate = _dto.FiscalDate;
        ledgerPostDraft.LedgerTag = _dto.LedgerTagId.HasValue ? await session.GetAsync<LedgerTag>(_dto.LedgerTagId.Value) : null;
        ledgerPostDraft.ContraTag = _dto.ContraTagId.HasValue ? await session.GetAsync<LedgerTag>(_dto.ContraTagId.Value) : null;
        return new QueryResult<LedgerPostDraft>(ledgerPostDraft);
    }
}