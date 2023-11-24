using System.Threading.Tasks;
using Anex.Api.Database.Commands.Abstract;
using Anex.Api.Database.Commands.Utilities;
using Anex.Api.Dto;
using Anex.Domain;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class UpdateLedgerPostDraftCommand : BaseUpdateEntityCommand<LedgerPostDraft>
{
    private readonly EditableLedgerPostDraftDto _dto;

    public UpdateLedgerPostDraftCommand(long id, int version, EditableLedgerPostDraftDto dto) 
        : base(id, version)
    {
        _dto = dto;
    }

    protected override async Task<CommandResult> TryUpdateEntity(ISession session, LedgerPostDraft entity)
    {
        entity.Amount = _dto.Amount;
        entity.VoucherNumber = _dto.VoucherNumber;
        entity.FiscalDate = _dto.FiscalDate;
        entity.LedgerTag = _dto.LedgerTagId.HasValue ? await session.GetAsync<LedgerTag>(_dto.LedgerTagId.Value) : null;
        entity.ContraTag = _dto.ContraTagId.HasValue ? await session.GetAsync<LedgerTag>(_dto.ContraTagId.Value) : null;
        return new CommandResult();
    }
}