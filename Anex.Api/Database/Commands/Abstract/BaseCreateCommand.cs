using System.Threading.Tasks;
using Anex.Api.Database.Commands.Utilities;
using Anex.Api.Database.Queries;
using Anex.Domain.Abstract;
using NHibernate;

namespace Anex.Api.Database.Commands.Abstract;

public abstract class BaseCreateCommand<T> : IExecutableCommand
    where T:IHasId, IIsValidatable
{
    public async Task<CommandResult> TryExecute(ISession session)
    {
        var query = await CreateEntity(session);
        if (!query.Success)
        {
            return new CommandResult(query.Errors);
        }

        var entity = query.Result!;
        if (!entity.IsValid(out var errors))
        {
            return new CommandResult(errors);
        }

        await session.SaveAsync(entity);
        AssignedId = entity.Id;
        return new CommandResult();
    }

    protected abstract Task<QueryResult<T>> CreateEntity(ISession session);
    public long? AssignedId { get; private set; }
}