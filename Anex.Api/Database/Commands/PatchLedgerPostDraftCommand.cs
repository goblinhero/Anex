using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Anex.Api.Database.Commands.Abstract;
using Anex.Api.Database.Commands.Utilities;
using Anex.Api.Utilities;
using Anex.Domain;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class PatchLedgerPostDraftCommand : BaseUpdateEntityCommand<LedgerPostDraft>
{
    private readonly Dictionary<string, JsonElement> _updates;

    public PatchLedgerPostDraftCommand(long id, int version, Dictionary<string, JsonElement> updates) 
        : base(id, version)
    {
        _updates = updates;
    }

    protected override async Task<CommandResult> TryUpdateEntity(ISession session, LedgerPostDraft entity)
    {
        var setter = new EntitySetter<LedgerPostDraft>(new DictionaryHelper(_updates), entity);
        setter.UpdateSimpleProperty(lpd => lpd.FiscalDate);
        setter.UpdateSimpleProperty(lpd => lpd.Amount);
        setter.UpdateSimpleProperty(lpd => lpd.VoucherNumber);
        await setter.UpdateComplexProperty(lpd => lpd.LedgerTag, session);
        await setter.UpdateComplexProperty(lpd => lpd.ContraTag, session);
        return new CommandResult();
    }
}