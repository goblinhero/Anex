using NHibernate.Event;
using Anex.Domain.Abstract;
using Anex.Api.Exceptions;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System;

namespace Anex.Api.Database.Listeners;

public class CheckTransactionalUpdateListener : IPreUpdateEventListener
{
    public Task<bool> OnPreUpdateAsync(PreUpdateEvent ev, CancellationToken cancellationToken)
    {
        return Task.FromResult(!cancellationToken.IsCancellationRequested && OnPreUpdate(ev));
    }

    public bool OnPreUpdate(PreUpdateEvent ev)
    {
        if (!(ev.Entity is ITransaction transaction)) return false;
        var updatedColumns = new List<string>();
        for (int i = 0; i < ev.Persister.PropertyNames.Length; i++)
        {
            if (ev.State == null || ev.OldState == null)
                continue;
            if (Equals(ev.State[i], ev.OldState[i])) continue;
            var property = ev.Persister.PropertyNames[i];
            updatedColumns.Add(property);
        }
        var notUpdateableColumns = updatedColumns.Except(transaction.UpdateableColumns).ToList();
        if (notUpdateableColumns.Any())
        {
            throw new InvalidTransactionalUpdateException($"You may not update transactional data for {transaction.GetType()}.{Environment.NewLine}Columns:{string.Join(Environment.NewLine, notUpdateableColumns)}", notUpdateableColumns);
        }
        return false;
    }
}
