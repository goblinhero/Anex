using System.Threading.Tasks;
using Anex.Api.Database.Commands.Abstract;
using Anex.Api.Database.Commands.Utilities;
using Anex.Api.Dto;
using Anex.Domain;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class UpdateLedgerDraftCommand : BaseUpdateEntityCommand<LedgerDraft>
{
    private readonly EditableLedgerDraftDto _dto;

    public UpdateLedgerDraftCommand(long id, int version, EditableLedgerDraftDto dto)
        : base(id, version)
    {
        _dto = dto;
    }

    protected override async Task<CommandResult> TryUpdateEntity(ISession session, LedgerDraft entity)
    {
        entity.Description = _dto.Description!;
        if (_dto.FiscalPeriodId.HasValue)
        {
            var fiscalPeriod = await session.GetAsync<FiscalPeriod>(_dto.FiscalPeriodId.Value);
            if (fiscalPeriod == null)
            {
                return new CommandResult(new[]{$"{nameof(FiscalPeriod)} not found with id: {_dto.FiscalPeriodId.Value}"});
            }
            entity.FiscalPeriod = fiscalPeriod;
        }
        return new CommandResult();
    }
}