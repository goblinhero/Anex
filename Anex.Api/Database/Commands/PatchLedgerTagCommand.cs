using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Anex.Api.Utilities;
using Anex.Domain;
using NHibernate;

namespace Anex.Api.Database.Commands;

public class PatchLedgerTagCommand : IExecutableCommand
{
    private readonly long _id;
    private readonly int _version;
    private readonly Dictionary<string, JsonElement> _updates;

    public PatchLedgerTagCommand(long id, int version, Dictionary<string,JsonElement> updates)
    {
        _id = id;
        _version = version;
        _updates = updates;
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
        
        return !TrySetProperties(ledgerTag, out var errors) || !ledgerTag.IsValid(out errors) 
            ? new CommandResult(errors) 
            : new CommandResult();
    }
    
    private bool TrySetProperties(LedgerTag entity, out IEnumerable<string> errors)
    {
        var setter = new EntitySetter<LedgerTag>(new DictionaryHelper(_updates), entity);
        setter.UpdateSimpleProperty(e => e.Description);
        setter.UpdateSimpleProperty(e => e.Number);
        errors = Array.Empty<string>();
        return true;
    }
}