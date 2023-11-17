using Anex.Api.Exceptions;
using Anex.Domain.Abstract;
using NHibernate.Event;
using System.Threading;
using System.Threading.Tasks;

namespace Anex.Api.Database.Listeners;

public class CheckValidityListener : IPreInsertEventListener, IPreUpdateEventListener
{
    public Task<bool> OnPreInsertAsync(PreInsertEvent ev, CancellationToken cancellationToken)
    {
        return Task.FromResult(!cancellationToken.IsCancellationRequested && OnPreInsert(ev));
    }

    public bool OnPreInsert(PreInsertEvent ev)
    {
        CheckValidity(ev);
        return false;
    }

    public Task<bool> OnPreUpdateAsync(PreUpdateEvent ev, CancellationToken cancellationToken)
    {
        return Task.FromResult(!cancellationToken.IsCancellationRequested && OnPreUpdate(ev));
    }

    public bool OnPreUpdate(PreUpdateEvent ev)
    {
        CheckValidity(ev);
        return false;
    }

    private void CheckValidity(IPreDatabaseOperationEventArgs ev)
    {
        if (!(ev.Entity is IIsValidatable)) return;
        var validatable = (IIsValidatable)ev.Entity;
        if (!validatable.IsValid(out var errors))
            throw new NonValidException(validatable, errors);
    }
}
