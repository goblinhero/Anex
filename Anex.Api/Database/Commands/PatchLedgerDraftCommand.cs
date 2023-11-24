using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Anex.Api.Database.Commands.Abstract;
using Anex.Api.Database.Commands.Utilities;
using Anex.Api.Utilities;
using Anex.Domain;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class PatchLedgerDraftCommand : BaseUpdateEntityCommand<LedgerDraft>
{
    private readonly Dictionary<string, JsonElement> _updates;

    public PatchLedgerDraftCommand(long id, int version, Dictionary<string, JsonElement> updates)
        : base(id, version)
    {
        _updates = updates;
    }

    protected override async Task<CommandResult> TryUpdateEntity(ISession session, LedgerDraft entity)
    {
        var setter = new EntitySetter<LedgerDraft>(new DictionaryHelper(_updates), entity);
        setter.UpdateSimpleProperty(e => e.Description);
        await setter.UpdateComplexProperty<FiscalPeriod>(e => e.FiscalPeriod, session);
        return new CommandResult();
    }
}