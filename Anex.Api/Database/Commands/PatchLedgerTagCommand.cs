using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Anex.Api.Database.Commands.Abstract;
using Anex.Api.Database.Commands.Utilities;
using Anex.Api.Utilities;
using Anex.Domain;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class PatchLedgerTagCommand : BaseUpdateEntityCommand<LedgerTag>
{
    private readonly Dictionary<string, JsonElement> _updates;

    public PatchLedgerTagCommand(long id, int version, Dictionary<string, JsonElement> updates)
        : base(id, version)
    {
        _updates = updates;
    }

    protected override Task<CommandResult> TryUpdateEntity(ISession session, LedgerTag entity)
    {
        var setter = new EntitySetter<LedgerTag>(new DictionaryHelper(_updates), entity);
        setter.UpdateSimpleProperty(e => e.Description);
        setter.UpdateSimpleProperty(e => e.Number);
        return Task.FromResult(new CommandResult());
    }
}