using Anex.Api.Exceptions;
using Anex.Domain.Abstract;
using NHibernate.Event;
using System.Threading;
using System.Threading.Tasks;

namespace Anex.Api.Database.Listeners;

public class DisallowDeleteTransactionListener : IPreDeleteEventListener
{
    public Task<bool> OnPreDeleteAsync(PreDeleteEvent ev, CancellationToken cancellationToken)
    {
        return Task.FromResult(!cancellationToken.IsCancellationRequested && OnPreDelete(ev));
    }

    public bool OnPreDelete(PreDeleteEvent ev)
    {
        if (ev.Entity is ITransaction transaction) throw new TransactionDeleteException(transaction);
        return false;
    }
}